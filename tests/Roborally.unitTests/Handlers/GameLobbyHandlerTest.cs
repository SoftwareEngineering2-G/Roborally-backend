using Moq;
using Roborally.core.application;
using Roborally.core.application.Contracts;
using Roborally.core.application.Handlers;
using Roborally.core.domain.Bases;
using Roborally.core.domain.Lobby;
using Roborally.core.domain.User;

namespace Roborally.unitTests.Handlers;

public class GameLobbyHandlerTest
{
    private readonly Mock<IGameLobbyRepository> _gameLobbyRepoMock;
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly Mock<ISystemTime> _systemTimeMock;
    private readonly CreateGameLobbyCommandHandler _handler;

    public GameLobbyHandlerTest()
    {
        _gameLobbyRepoMock = new Mock<IGameLobbyRepository>();
        _userRepoMock = new Mock<IUserRepository>();
        _uowMock = new Mock<IUnitOfWork>();
        _systemTimeMock = new Mock<ISystemTime>();
        
        _systemTimeMock.Setup(x => x.CurrentTime).Returns(DateTime.UtcNow);
        
        _handler = new CreateGameLobbyCommandHandler(
            _gameLobbyRepoMock.Object,
            _userRepoMock.Object,
            _uowMock.Object,
            _systemTimeMock.Object);
    }

    [Fact]
    public async Task CannotCreateLobby_WhenHostUserNotFound()
    {
        // Arrange
        _userRepoMock.Setup(r => r.FindAsync(It.IsAny<Guid>()))
            .ReturnsAsync((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => _handler.ExecuteAsync(new CreateGameLobbyCommand
        {
            HostUserId = Guid.NewGuid(),
            IsPrivate = false,
            GameRoomName = "My Game"
        }, CancellationToken.None));
    }
    
    [Fact]
    public async Task CannotCreateLobby_WhenHostAlreadyHostingActiveLobby()
    {
        // Arrange
        var hostUser = new User
        {
            Id = Guid.NewGuid(),
            Username = "ValidHost",
            Password = "ValidPassword123",
            Birthday = DateOnly.FromDateTime(DateTime.Now.AddYears(-20))
        };

        _userRepoMock.Setup(r => r.FindAsync(hostUser.Id))
            .ReturnsAsync(hostUser);

        // User is already hosting an active lobby
        _gameLobbyRepoMock.Setup(r => r.IsUserCurrentlyHostingActiveLobbyAsync(hostUser.Id))
            .ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => _handler.ExecuteAsync(new CreateGameLobbyCommand
        {
            HostUserId = hostUser.Id,
            IsPrivate = false,
            GameRoomName = "New Lobby"
        }, CancellationToken.None));
    }
    
    [Fact]
    public async Task CannotCreateLobby_WhenRoomNameInvalid()
    {
        // Arrange
        var hostUser = new User
        {
            Id = Guid.NewGuid(),
            Username = "ValidHost",
            Password = "ValidPassword123",
            Birthday = DateOnly.FromDateTime(DateTime.Now.AddYears(-20))
        };

        _userRepoMock.Setup(r => r.FindAsync(hostUser.Id))
            .ReturnsAsync(hostUser);

        _gameLobbyRepoMock.Setup(r => r.IsUserCurrentlyHostingActiveLobbyAsync(hostUser.Id))
            .ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => _handler.ExecuteAsync(new CreateGameLobbyCommand
        {
            HostUserId = hostUser.Id,
            IsPrivate = false,
            GameRoomName = " " // Invalid room name
        }, CancellationToken.None));
    }
    
    [Fact]
    public async Task CanCreatePublicLobby_WithValidData()
    {
        // Arrange
        var hostUser = new User
        {
            Id = Guid.NewGuid(),
            Username = "ValidHost",
            Password = "ValidPassword123",
            Birthday = DateOnly.FromDateTime(DateTime.Now.AddYears(-20))
        };

        _userRepoMock.Setup(r => r.FindAsync(hostUser.Id))
            .ReturnsAsync(hostUser);

        _gameLobbyRepoMock.Setup(r => r.IsUserCurrentlyHostingActiveLobbyAsync(hostUser.Id))
            .ReturnsAsync(false);

        // Act
        var resultId = await _handler.ExecuteAsync(new CreateGameLobbyCommand
        {
            HostUserId = hostUser.Id,
            IsPrivate = false, // Public lobby
            GameRoomName = "Public Game"
        }, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, resultId);
        _gameLobbyRepoMock.Verify(r => r.AddAsync(It.Is<GameLobby>(lobby => 
            lobby.HostId == hostUser.Id && 
            !lobby.IsPrivate && 
            lobby.GameRoomName == "Public Game" &&
            lobby.IsActive), It.IsAny<CancellationToken>()), Times.Once);
        _uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task CanCreatePrivateLobby_WithValidData()
    {
        // Arrange
        var hostUser = new User
        {
            Id = Guid.NewGuid(),
            Username = "ValidHost",
            Password = "ValidPassword123",
            Birthday = DateOnly.FromDateTime(DateTime.Now.AddYears(-20))
        };

        _userRepoMock.Setup(r => r.FindAsync(hostUser.Id))
            .ReturnsAsync(hostUser);

        _gameLobbyRepoMock.Setup(r => r.IsUserCurrentlyHostingActiveLobbyAsync(hostUser.Id))
            .ReturnsAsync(false);

        // Act
        var resultId = await _handler.ExecuteAsync(new CreateGameLobbyCommand
        {
            HostUserId = hostUser.Id,
            IsPrivate = true, // Private lobby
            GameRoomName = "Private Game"
        }, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, resultId);
        _gameLobbyRepoMock.Verify(r => r.AddAsync(It.Is<GameLobby>(lobby => 
            lobby.HostId == hostUser.Id && 
            lobby.IsPrivate && 
            lobby.GameRoomName == "Private Game" &&
            lobby.IsActive), It.IsAny<CancellationToken>()), Times.Once);
        _uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CanCreateNewLobby_AfterPreviousGameStarted()
    {
        // Arrange
        var hostUser = new User
        {
            Id = Guid.NewGuid(),
            Username = "ValidHost",
            Password = "ValidPassword123",
            Birthday = DateOnly.FromDateTime(DateTime.Now.AddYears(-20))
        };

        _userRepoMock.Setup(r => r.FindAsync(hostUser.Id))
            .ReturnsAsync(hostUser);

        // User had a lobby before, but it has started (no longer active)
        _gameLobbyRepoMock.Setup(r => r.IsUserCurrentlyHostingActiveLobbyAsync(hostUser.Id))
            .ReturnsAsync(false);

        // Act
        var resultId = await _handler.ExecuteAsync(new CreateGameLobbyCommand
        {
            HostUserId = hostUser.Id,
            IsPrivate = false,
            GameRoomName = "New Game After Previous"
        }, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, resultId);
        _gameLobbyRepoMock.Verify(r => r.AddAsync(It.IsAny<GameLobby>(), It.IsAny<CancellationToken>()), Times.Once);
        _uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
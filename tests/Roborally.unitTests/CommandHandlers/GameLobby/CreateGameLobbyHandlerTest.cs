using Moq;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.CommandContracts;
using Roborally.core.application.CommandHandlers.GameLobby;
using Roborally.core.domain;
using Roborally.core.domain.Bases;
using Roborally.core.domain.User;

namespace Roborally.unitTests.CommandHandlers.GameLobby;

public class CreateGameLobbyHandlerTest
{
    private readonly Mock<IGameLobbyRepository> _gameLobbyRepoMock;
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly Mock<ISystemTime> _systemTimeMock;
    private readonly CreateGameLobbyCommandHandler _handler;

    public CreateGameLobbyHandlerTest()
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
        _userRepoMock.Setup(r => r.FindAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => _handler.ExecuteAsync(new CreateGameLobbyCommand
        {
            HostUsername = "NonExistentUser",
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
            Username = "ValidHost",
            Password = "ValidPassword123",
            Birthday = DateOnly.FromDateTime(DateTime.Now.AddYears(-20))
        };

        _userRepoMock.Setup(r => r.FindAsync(hostUser.Username, It.IsAny<CancellationToken>()))
            .ReturnsAsync(hostUser);

        // User is already hosting an active lobby
        _gameLobbyRepoMock.Setup(r => r.IsUserCurrentlyHostingActiveLobbyAsync(hostUser.Username))
            .ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => _handler.ExecuteAsync(new CreateGameLobbyCommand
        {
            HostUsername = hostUser.Username,
            IsPrivate = false,
            GameRoomName = "NewShuffled Lobby"
        }, CancellationToken.None));
    }
    
    [Fact]
    public async Task CannotCreateLobby_WhenRoomNameInvalid()
    {
        // Arrange
        var hostUser = new User
        {
            Username = "ValidHost",
            Password = "ValidPassword123",
            Birthday = DateOnly.FromDateTime(DateTime.Now.AddYears(-20))
        };

        _userRepoMock.Setup(r => r.FindAsync(hostUser.Username, It.IsAny<CancellationToken>()))
            .ReturnsAsync(hostUser);

        _gameLobbyRepoMock.Setup(r => r.IsUserCurrentlyHostingActiveLobbyAsync(hostUser.Username))
            .ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => _handler.ExecuteAsync(new CreateGameLobbyCommand
        {
            HostUsername = hostUser.Username,
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
            Username = "ValidHost",
            Password = "ValidPassword123",
            Birthday = DateOnly.FromDateTime(DateTime.Now.AddYears(-20))
        };

        _userRepoMock.Setup(r => r.FindAsync(hostUser.Username, It.IsAny<CancellationToken>()))
            .ReturnsAsync(hostUser);

        _gameLobbyRepoMock.Setup(r => r.IsUserCurrentlyHostingActiveLobbyAsync(hostUser.Username))
            .ReturnsAsync(false);

        // Act
        var resultId = await _handler.ExecuteAsync(new CreateGameLobbyCommand
        {
            HostUsername = hostUser.Username,
            IsPrivate = false, // Public lobby
            GameRoomName = "Public Game"
        }, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, resultId);
        _gameLobbyRepoMock.Verify(r => r.AddAsync(It.Is<core.domain.Lobby.GameLobby>(lobby => 
            lobby.HostUsername == hostUser.Username && 
            !lobby.IsPrivate && 
            lobby.Name == "Public Game" &&
            lobby.IsActive), It.IsAny<CancellationToken>()), Times.Once);
        _uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task CanCreatePrivateLobby_WithValidData()
    {
        // Arrange
        var hostUser = new User
        {
            Username = "ValidHost",
            Password = "ValidPassword123",
            Birthday = DateOnly.FromDateTime(DateTime.Now.AddYears(-20))
        };

        _userRepoMock.Setup(r => r.FindAsync(hostUser.Username, It.IsAny<CancellationToken>()))
            .ReturnsAsync(hostUser);

        _gameLobbyRepoMock.Setup(r => r.IsUserCurrentlyHostingActiveLobbyAsync(hostUser.Username))
            .ReturnsAsync(false);

        // Act
        var resultId = await _handler.ExecuteAsync(new CreateGameLobbyCommand
        {
            HostUsername = hostUser.Username,
            IsPrivate = true, // Private lobby
            GameRoomName = "Private Game"
        }, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, resultId);
        _gameLobbyRepoMock.Verify(r => r.AddAsync(It.Is<core.domain.Lobby.GameLobby>(lobby => 
            lobby.HostUsername == hostUser.Username && 
            lobby.IsPrivate && 
            lobby.Name == "Private Game" &&
            lobby.IsActive), It.IsAny<CancellationToken>()), Times.Once);
        _uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CanCreateNewLobby_AfterPreviousGameStarted()
    {
        // Arrange
        var hostUser = new User
        {
            Username = "ValidHost",
            Password = "ValidPassword123",
            Birthday = DateOnly.FromDateTime(DateTime.Now.AddYears(-20))
        };

        _userRepoMock.Setup(r => r.FindAsync(hostUser.Username, It.IsAny<CancellationToken>()))
            .ReturnsAsync(hostUser);

        // User had a lobby before, but it has started (no longer active)
        _gameLobbyRepoMock.Setup(r => r.IsUserCurrentlyHostingActiveLobbyAsync(hostUser.Username))
            .ReturnsAsync(false);

        // Act
        var resultId = await _handler.ExecuteAsync(new CreateGameLobbyCommand
        {
            HostUsername = hostUser.Username,
            IsPrivate = false,
            GameRoomName = "NewShuffled Game After Previous"
        }, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, resultId);
        _gameLobbyRepoMock.Verify(r => r.AddAsync(It.IsAny<core.domain.Lobby.GameLobby>(), It.IsAny<CancellationToken>()), Times.Once);
        _uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
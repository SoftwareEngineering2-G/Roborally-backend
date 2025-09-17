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
    [Fact]
    public async Task CannotCreateLobby_WhenHostUserNotFound()
    {
        // Arrange
        var gameLobbyRepoMock = new Mock<IGameLobbyRepository>();
        var userRepoMock = new Mock<IUserRepository>();
        var uowMock = new Mock<IUnitOfWork>();

        var handler = new CreateGameLobbyCommandHandler(
            gameLobbyRepoMock.Object,
            userRepoMock.Object,
            uowMock.Object);

        // Host user not found
        userRepoMock.Setup(r => r.FindAsync(It.IsAny<Guid>()))
            .ReturnsAsync((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => handler.ExecuteAsync(new CreateGameLobbyCommand
        {
            HostUserId = Guid.NewGuid(),
            IsPrivate = false,
            GameRoomName = "My Game"
        }, CancellationToken.None));
    }
    
    [Fact]
    public async Task CannotCreateLobby_WhenHostAlreadyHosting()
    {
        // Arrange
        var gameLobbyRepoMock = new Mock<IGameLobbyRepository>();
        var userRepoMock = new Mock<IUserRepository>();
        var uowMock = new Mock<IUnitOfWork>();

        var handler = new CreateGameLobbyCommandHandler(
            gameLobbyRepoMock.Object,
            userRepoMock.Object,
            uowMock.Object);

        var hostUser = new User
        {
            Id = Guid.NewGuid(),
            Username = "ValidHost",
            Password = "ValidPassword123",
            Birthday = DateOnly.FromDateTime(DateTime.Now.AddYears(-20))
        };

        // Host user found
        userRepoMock.Setup(r => r.FindAsync(hostUser.Id))
            .ReturnsAsync(hostUser);

        // Already hosting
        gameLobbyRepoMock.Setup(r => r.FindByHostIdAsync(hostUser.Id))
            .ReturnsAsync(new GameLobby(hostUser, false, "Existing Lobby"));

        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => handler.ExecuteAsync(new CreateGameLobbyCommand
        {
            HostUserId = hostUser.Id,
            IsPrivate = false,
            GameRoomName = "New Lobby"
        }, CancellationToken.None));
    }
    
    [Fact]
    public async Task CannotCreateLobby_WhenRoomNameEmpty()
    {
        // Arrange
        var gameLobbyRepoMock = new Mock<IGameLobbyRepository>();
        var userRepoMock = new Mock<IUserRepository>();
        var uowMock = new Mock<IUnitOfWork>();

        var handler = new CreateGameLobbyCommandHandler(
            gameLobbyRepoMock.Object,
            userRepoMock.Object,
            uowMock.Object);

        var hostUser = new User
        {
            Id = Guid.NewGuid(),
            Username = "ValidHost",
            Password = "ValidPassword123",
            Birthday = DateOnly.FromDateTime(DateTime.Now.AddYears(-20))
        };

        // Host user found
        userRepoMock.Setup(r => r.FindAsync(hostUser.Id))
            .ReturnsAsync(hostUser);

        // No existing lobby
        gameLobbyRepoMock.Setup(r => r.FindByHostIdAsync(hostUser.Id))
            .ReturnsAsync((GameLobby?)null);

        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => handler.ExecuteAsync(new CreateGameLobbyCommand
        {
            HostUserId = hostUser.Id,
            IsPrivate = false,
            GameRoomName = " "
        }, CancellationToken.None));
    }
    
    [Fact]
    public async Task CanCreateLobby_WithValidData()
    {
        // Arrange
        var gameLobbyRepoMock = new Mock<IGameLobbyRepository>();
        var userRepoMock = new Mock<IUserRepository>();
        var uowMock = new Mock<IUnitOfWork>();

        var handler = new CreateGameLobbyCommandHandler(
            gameLobbyRepoMock.Object,
            userRepoMock.Object,
            uowMock.Object);

        var hostUser = new User
        {
            Id = Guid.NewGuid(),
            Username = "ValidHost",
            Password = "ValidPassword123",
            Birthday = DateOnly.FromDateTime(DateTime.Now.AddYears(-20))
        };

        // Host user found
        userRepoMock.Setup(r => r.FindAsync(hostUser.Id))
            .ReturnsAsync(hostUser);

        // No existing lobby
        gameLobbyRepoMock.Setup(r => r.FindByHostIdAsync(hostUser.Id))
            .ReturnsAsync((GameLobby?)null);

        // Act
        var resultId = await handler.ExecuteAsync(new CreateGameLobbyCommand
        {
            HostUserId = hostUser.Id,
            IsPrivate = true,
            GameRoomName = "Private Game"
        }, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, resultId);
        gameLobbyRepoMock.Verify(r => r.AddAsync(It.IsAny<GameLobby>(), It.IsAny<CancellationToken>()), Times.Once);
        uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    

}
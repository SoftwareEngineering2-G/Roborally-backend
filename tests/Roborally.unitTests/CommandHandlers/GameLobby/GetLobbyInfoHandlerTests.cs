using Moq;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.CommandContracts;
using Roborally.core.application.CommandHandlers.GameLobby;
using Roborally.core.domain;
using Roborally.core.domain.Bases;
using Roborally.unitTests.Factory;

namespace Roborally.unitTests.CommandHandlers.GameLobby;

public class GetLobbyInfoHandlerTests
{
    private readonly Mock<IGameLobbyRepository> _gameLobbyRepositoryMock;
    private readonly Mock<IGameRepository> _gameRepositoryMock;
    private readonly GetLobbyInfoCommandHandler _handler;

    public GetLobbyInfoHandlerTests()
    {
        _gameLobbyRepositoryMock = new Mock<IGameLobbyRepository>();
        _gameRepositoryMock = new Mock<IGameRepository>();
        _handler = new GetLobbyInfoCommandHandler(_gameLobbyRepositoryMock.Object, _gameRepositoryMock.Object);
    }

    [Fact]
    public async Task CannotGetLobbyInfo_WhenLobbyDoesNotExist()
    {
        // Arrange
        _gameLobbyRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>()))
            .ReturnsAsync((core.domain.Lobby.GameLobby?)null);

        var command = new GetLobbyInfoCommand
        {
            GameId = Guid.NewGuid(),
            Username = "Test User"
        };

        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => _handler.ExecuteAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task CannotGetLobbyInfo_WhenUserIsNotInLobby()
    {
        // Arrange
        var joinedUser = UserFactory.GetValidUser();
        var systemTimeMock = new Mock<ISystemTime>();
        var lobby = GameLobbyFactory.GetValidGameLobby(joinedUser, systemTimeMock.Object);

        _gameLobbyRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>()))
            .ReturnsAsync(lobby);

        var command = new GetLobbyInfoCommand
        {
            GameId = lobby.GameId,
            Username = "SomeOtherUser"
        };

        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => _handler.ExecuteAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task CannotGetLobbyInfo_WhenUserIsNotInRequiredUsers()
    {
        // Arrange
        var joinedUser = UserFactory.GetValidUser();
        var requiredUser = UserFactory.GetValidUser();
        var systemTimeMock = new Mock<ISystemTime>();
        var lobby = GameLobbyFactory.GetValidGameLobby(joinedUser, systemTimeMock.Object);
        lobby.InitLobbyToContinue([requiredUser]);
        lobby.JoinLobby(requiredUser);

        _gameLobbyRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>()))
            .ReturnsAsync(lobby);

        var command = new GetLobbyInfoCommand
        {
            GameId = lobby.GameId,
            Username = joinedUser.Username // User is in joined users but not in required users
        };

        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => _handler.ExecuteAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task CannotGetLobbyInfo_WhenGameDoesNotExist()
    {
        // Arrange
        var joinedUser = UserFactory.GetValidUser();
        var requiredUser = UserFactory.GetValidUser();
        var systemTimeMock = new Mock<ISystemTime>();
        var lobby = GameLobbyFactory.GetValidGameLobby(joinedUser, systemTimeMock.Object);
        lobby.InitLobbyToContinue([requiredUser]);
        lobby.JoinLobby(requiredUser);

        _gameLobbyRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>()))
            .ReturnsAsync(lobby);

        var command = new GetLobbyInfoCommand
        {
            GameId = lobby.GameId,
            Username = requiredUser.Username
        };

        _gameRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((core.domain.Game.Game?)null);

        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => _handler.ExecuteAsync(command, CancellationToken.None));
        _gameLobbyRepositoryMock.Verify(repo => repo.FindAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task CannotGetLobbyInfo_WhenGameIsNotPaused()
    {
        // Arrange
        var joinedUser = UserFactory.GetValidUser();
        var requiredUser = UserFactory.GetValidUser();
        var systemTimeMock = new Mock<ISystemTime>();
        var lobby = GameLobbyFactory.GetValidGameLobby(joinedUser, systemTimeMock.Object);
        lobby.InitLobbyToContinue([requiredUser]);
        lobby.JoinLobby(requiredUser);

        _gameLobbyRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>()))
            .ReturnsAsync(lobby);

        var command = new GetLobbyInfoCommand
        {
            GameId = lobby.GameId,
            Username = requiredUser.Username
        };

        var game = GameFactory.GetValidGame();
        game.IsPaused = false;

        _gameRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(game);

        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => _handler.ExecuteAsync(command, CancellationToken.None));
        _gameRepositoryMock.Verify(repo => repo.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetLobbyInfo_WhenRequiredUsersPresent()
    {
        // Arrange
        var joinedUser = UserFactory.GetValidUser();
        var requiredUser = UserFactory.GetValidUser();
        var systemTimeMock = new Mock<ISystemTime>();
        var lobby = GameLobbyFactory.GetValidGameLobby(joinedUser, systemTimeMock.Object);
        lobby.InitLobbyToContinue([requiredUser]);
        lobby.JoinLobby(requiredUser);

        _gameLobbyRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>()))
            .ReturnsAsync(lobby);

        var command = new GetLobbyInfoCommand
        {
            GameId = lobby.GameId,
            Username = requiredUser.Username
        };

        var game = GameFactory.GetValidGame();
        game.IsPaused = true;

        _gameRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(game);

        // Act
        await _handler.ExecuteAsync(command, CancellationToken.None);

        // Assert
        _gameRepositoryMock.Verify(repo => repo.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetLobbyInfo_WhenNoRequiredUsers()
    {
        // Arrange
        var joinedUser = UserFactory.GetValidUser();
        var systemTimeMock = new Mock<ISystemTime>();
        var lobby = GameLobbyFactory.GetValidGameLobby(joinedUser, systemTimeMock.Object);

        _gameLobbyRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>()))
            .ReturnsAsync(lobby);

        var command = new GetLobbyInfoCommand
        {
            GameId = lobby.GameId,
            Username = joinedUser.Username
        };

        // Act
        await _handler.ExecuteAsync(command, CancellationToken.None);

        // Assert
        _gameRepositoryMock.Verify(repo => repo.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
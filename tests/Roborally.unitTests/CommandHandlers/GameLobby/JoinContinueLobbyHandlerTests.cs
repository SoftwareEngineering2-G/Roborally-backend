using Moq;
using Roborally.core.application.ApplicationContracts.Broadcasters;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.CommandContracts;
using Roborally.core.application.CommandHandlers.GameLobby;
using Roborally.core.domain;
using Roborally.core.domain.Bases;
using Roborally.unitTests.Factory;

namespace Roborally.unitTests.CommandHandlers.GameLobby;

public class JoinContinueLobbyHandlerTests
{
    private readonly Mock<IGameLobbyRepository> _gameLobbyRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IGameRepository> _gameRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IGameLobbyBroadcaster> _gameLobbyBroadcasterMock;
    private readonly JoinContinueLobbyCommandHandler _handler;

    public JoinContinueLobbyHandlerTests()
    {
        _gameLobbyRepositoryMock = new Mock<IGameLobbyRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _gameRepositoryMock = new Mock<IGameRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _gameLobbyBroadcasterMock = new Mock<IGameLobbyBroadcaster>();

        _handler = new JoinContinueLobbyCommandHandler(
            _gameLobbyRepositoryMock.Object,
            _userRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _gameRepositoryMock.Object,
            _gameLobbyBroadcasterMock.Object
        );
    }

    [Fact]
    public async Task CannotJoinLobby_WhenUserDoesNotExist()
    {
        // Arrange
        _userRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((core.domain.User.User?)null);

        var command = new JoinContinueLobbyCommand
        {
            GameId = Guid.NewGuid(),
            Username = "ValidUsername",
        };

        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => _handler.ExecuteAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task CannotJoinLobby_WhenLobbyDoesNotExist()
    {
        // Arrange
        var user = UserFactory.GetValidUser();
        _userRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _gameLobbyRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>()))
            .ReturnsAsync((core.domain.Lobby.GameLobby?)null);

        var command = new JoinContinueLobbyCommand()
        {
            GameId = Guid.NewGuid(),
            Username = user.Username
        };

        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => _handler.ExecuteAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task CannotJoinLobby_WhenGameDoesNotExist()
    {
        // Arrange
        var joinedUser = UserFactory.GetValidUser();
        var systemTimeMock = new Mock<ISystemTime>();
        var lobby = GameLobbyFactory.GetValidGameLobby(joinedUser, systemTimeMock.Object);

        var newUser = UserFactory.GetValidUser();

        _userRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(newUser);

        _gameLobbyRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>()))
            .ReturnsAsync(lobby);

        _gameRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((core.domain.Game.Game?)null);

        var command = new JoinContinueLobbyCommand()
        {
            GameId = lobby.GameId,
            Username = newUser.Username
        };

        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => _handler.ExecuteAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task CannotJoinLobby_WhenGameIsNotPaused()
    {
        // Arrange
        var joinedUser = UserFactory.GetValidUser();
        var systemTimeMock = new Mock<ISystemTime>();
        var lobby = GameLobbyFactory.GetValidGameLobby(joinedUser, systemTimeMock.Object);

        var newUser = UserFactory.GetValidUser();

        _userRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(newUser);

        _gameLobbyRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>()))
            .ReturnsAsync(lobby);

        var game = GameFactory.GetValidGame();
        game.IsPaused = false;

        _gameRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(game);

        var command = new JoinContinueLobbyCommand()
        {
            GameId = lobby.GameId,
            Username = newUser.Username
        };

        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => _handler.ExecuteAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task CanJoinLobby_WhenNoJoinedUsers()
    {
        // Arrange
        var user1 = UserFactory.GetValidUser();
        var user2 = UserFactory.GetValidUser();
        var systemTimeMock = new Mock<ISystemTime>();
        var lobby = GameLobbyFactory.GetValidGameLobby(user1, systemTimeMock.Object);
        lobby.JoinLobby(user2);

        var game = GameFactory.GetGameFromLobby(lobby);
        game.IsPaused = true;

        _gameRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(game);

        lobby.LeaveLobby(user1);
        lobby.LeaveLobby(user2);
        _gameLobbyRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>()))
            .ReturnsAsync(lobby);

        _userRepositoryMock.Setup(repo => repo.FindAsync(user1.Username, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user1);
        _userRepositoryMock.Setup(repo => repo.FindAsync(user2.Username, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user2);

        var command = new JoinContinueLobbyCommand()
        {
            GameId = lobby.GameId,
            Username = user2.Username
        };

        // Act
        await _handler.ExecuteAsync(command, CancellationToken.None);

        // Assert
        _userRepositoryMock.Verify(repo => repo.FindAsync(user1.Username, It.IsAny<CancellationToken>()), Times.Once);
        _userRepositoryMock.Verify(repo => repo.FindAsync(user2.Username, It.IsAny<CancellationToken>()),
            Times.Exactly(2));
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _gameLobbyBroadcasterMock.Verify(broadcaster =>
                broadcaster.BroadcastUserJoinedAsync(command.GameId, command.Username, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
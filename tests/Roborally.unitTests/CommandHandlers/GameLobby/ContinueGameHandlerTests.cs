using Moq;
using Roborally.core.application.ApplicationContracts.Broadcasters;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.CommandContracts;
using Roborally.core.application.CommandHandlers.GameLobby;
using Roborally.core.domain;
using Roborally.core.domain.Bases;
using Roborally.unitTests.Factory;

namespace Roborally.unitTests.CommandHandlers.GameLobby;

public class ContinueGameHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IGameLobbyRepository> _gameLobbyRepositoryMock;
    private readonly Mock<IGameRepository> _gameRepositoryMock;
    private readonly Mock<IGameBoardRepository> _gameBoardRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ISystemTime> _systemTimeMock;
    private readonly Mock<IGameLobbyBroadcaster> _gameLobbyBroadcasterMock;
    private readonly ContinueGameCommandHandler _handler;

    public ContinueGameHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _gameLobbyRepositoryMock = new Mock<IGameLobbyRepository>();
        _gameRepositoryMock = new Mock<IGameRepository>();
        _gameBoardRepositoryMock = new Mock<IGameBoardRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _systemTimeMock = new Mock<ISystemTime>();
        _gameLobbyBroadcasterMock = new Mock<IGameLobbyBroadcaster>();

        _handler = new ContinueGameCommandHandler(
            _userRepositoryMock.Object,
            _gameLobbyRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _systemTimeMock.Object,
            _gameLobbyBroadcasterMock.Object,
            _gameRepositoryMock.Object,
            _gameBoardRepositoryMock.Object);
    }

    [Fact]
    public async Task CannotContinueGame_WhenUserDoesNotExist()
    {
        // Arrange
        _userRepositoryMock.Setup(repo => repo.ExistsByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var command = new ContinueGameCommand
        {
            GameId = Guid.NewGuid(),
            Username = "NonExistentUser"
        };

        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => _handler.ExecuteAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task CannotContinueGame_WhenLobbyDoesNotExist()
    {
        // Arrange
        _userRepositoryMock.Setup(repo => repo.ExistsByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _gameLobbyRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>()))
            .ReturnsAsync((core.domain.Lobby.GameLobby?)null);

        var command = new ContinueGameCommand
        {
            GameId = Guid.NewGuid(),
            Username = "ExistingUser"
        };

        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => _handler.ExecuteAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task CannotContinueGame_WhenGameDoesNotExist()
    {
        // Arrange
        _userRepositoryMock.Setup(repo => repo.ExistsByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var user = UserFactory.GetValidUser();
        var lobby = GameLobbyFactory.GetValidGameLobby(user, _systemTimeMock.Object);
        _gameLobbyRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>()))
            .ReturnsAsync(lobby);

        _gameRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((core.domain.Game.Game?)null);

        var command = new ContinueGameCommand
        {
            GameId = Guid.NewGuid(),
            Username = user.Username
        };

        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => _handler.ExecuteAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task ContinueGame_WhenUserExists_AndLobbyExists_AndGameExists()
    {
        // Arrange
        var user = UserFactory.GetValidUser();
        var lobby = GameLobbyFactory.GetValidGameLobby(user, _systemTimeMock.Object);
        var game = GameFactory.GetValidGame();
        game.IsPaused = true;
        
        _userRepositoryMock.Setup(repo => repo.ExistsByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _gameLobbyRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>()))
            .ReturnsAsync(lobby);

        _gameRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(game);

        var command = new ContinueGameCommand
        {
            GameId = game.GameId,
            Username = lobby.HostUsername
        };

        // Act
        await _handler.ExecuteAsync(command, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _gameLobbyBroadcasterMock.Verify(
            broadcaster => broadcaster.BroadcastGameContinuedAsync(game.GameId, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
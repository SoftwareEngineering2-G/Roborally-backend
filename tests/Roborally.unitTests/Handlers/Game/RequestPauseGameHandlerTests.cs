using Moq;
using Roborally.core.application.ApplicationContracts.Broadcasters;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.CommandContracts.Game;
using Roborally.core.application.CommandHandlers.Game;
using Roborally.core.domain;
using Roborally.core.domain.Bases;
using Roborally.unitTests.Factory;

namespace Roborally.unitTests.Handlers.Game;

public class RequestPauseGameHandlerTests
{
    private readonly Mock<IGameRepository> _gameRepositoryMock;
    private readonly Mock<IGameBroadcaster> _gameBroadcasterMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ISystemTime> _systemTimeMock;
    private readonly RequestPauseGameCommandHandler _handler;

    public RequestPauseGameHandlerTests()
    {
        _gameRepositoryMock = new Mock<IGameRepository>();
        _gameBroadcasterMock = new Mock<IGameBroadcaster>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _systemTimeMock = new Mock<ISystemTime>();

        _handler = new RequestPauseGameCommandHandler(_gameRepositoryMock.Object, _gameBroadcasterMock.Object,
            _unitOfWorkMock.Object, _systemTimeMock.Object);
    }

    [Fact]
    public async Task CannotRequestPauseGame_WhenGameDoesNotExist()
    {
        // Arrange
        _gameRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((core.domain.Game.Game?)null);

        var command = new RequestPauseGameCommand
        {
            GameId = Guid.NewGuid(),
            RequesterUsername = "SomePlayer"
        };

        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => _handler.ExecuteAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task RequestPauseGame_WhenGameExists()
    {
        // Arrange
        var game = GameFactory.GetValidGame();
        _gameRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(game);

        var command = new RequestPauseGameCommand
        {
            GameId = game.GameId,
            RequesterUsername = game.Players[0].Username
        };

        // Act
        await _handler.ExecuteAsync(command, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _gameBroadcasterMock.Verify(
            g => g.BroadcastPauseGameRequestedAsync(game.GameId, command.RequesterUsername,
                It.IsAny<CancellationToken>()), Times.Once);
    }
}
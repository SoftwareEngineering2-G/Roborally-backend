using Moq;
using Roborally.core.application.ApplicationContracts.Broadcasters;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.CommandContracts.Game;
using Roborally.core.application.CommandHandlers.Game;
using Roborally.core.domain;
using Roborally.core.domain.Bases;
using Roborally.core.domain.Game;
using Roborally.unitTests.Factory;

namespace Roborally.unitTests.CommandHandlers.Game;

public class StartActivationPhaseHandlerTests
{
    private readonly Mock<IGameRepository> _gameRepositoryMock;
    private readonly Mock<IGameBroadcaster> _gameBroadcasterMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly StartActivationPhaseCommandHandler _handler;

    public StartActivationPhaseHandlerTests()
    {
        _gameRepositoryMock = new Mock<IGameRepository>();
        _gameBroadcasterMock = new Mock<IGameBroadcaster>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _handler = new StartActivationPhaseCommandHandler(_gameRepositoryMock.Object, _gameBroadcasterMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task CannotStartActivationPhase_WhenGameDoesNotExist()
    {
        // Arrange
        _gameRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((core.domain.Game.Game?)null);

        var command = new StartActivationPhaseCommand
        {
            GameId = Guid.NewGuid(),
            Username = "SomePlayer"
        };

        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => _handler.ExecuteAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task StartActivationPhase_WhenGameExists()
    {
        // Arrange
        var game = GameFactory.GetValidGame();
        game.CurrentPhase = GamePhase.ProgrammingPhase;
        _gameRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(game);

        var command = new StartActivationPhaseCommand
        {
            GameId = game.GameId,
            Username = game.Players[0].Username
        };

        // Act
        await _handler.ExecuteAsync(command, CancellationToken.None);

        // Assert
        Assert.Equal(game.CurrentPhase, GamePhase.ActivationPhase);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _gameBroadcasterMock.Verify(
            broadcaster => broadcaster.BroadcastActivationPhaseStartedAsync(game.GameId, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
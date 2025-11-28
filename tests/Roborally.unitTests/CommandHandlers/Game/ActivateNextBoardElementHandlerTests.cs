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

public class ActivateBoardElementHandlerTests
{
    private readonly Mock<IGameRepository> _gameRepositoryMock;
    private readonly Mock<IGameBroadcaster> _gameBroadcasterMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ISystemTime> _systemTimeMock;
    private readonly ActivateNextBoardElementCommandHandler _handler;

    public ActivateBoardElementHandlerTests()
    {
        _gameRepositoryMock = new Mock<IGameRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _gameBroadcasterMock = new Mock<IGameBroadcaster>();
        _systemTimeMock = new Mock<ISystemTime>();

        _handler = new ActivateNextBoardElementCommandHandler(_gameRepositoryMock.Object, _gameBroadcasterMock.Object,
            _unitOfWorkMock.Object, _systemTimeMock.Object);
    }

    [Fact]
    public void CannotActivateBoardElement_WhenGameDoesNotExist()
    {
        // Arrange
        _gameRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((core.domain.Game.Game?)null);

        var command = new ActivateNextBoardElementCommand
        {
            GameId = Guid.NewGuid()
        };

        // Act & Assert
        var exception =
            Assert.ThrowsAsync<CustomException>(() => _handler.ExecuteAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task ActivateBoardElement_WhenGameExists()
    {
        // Arrange
        var game = GameFactory.GetValidGame();
        game.CurrentPhase = GamePhase.ActivationPhase;
        _gameRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(game);

        var command = new ActivateNextBoardElementCommand
        {
            GameId = Guid.NewGuid()
        };

        // Act
        await _handler.ExecuteAsync(command, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
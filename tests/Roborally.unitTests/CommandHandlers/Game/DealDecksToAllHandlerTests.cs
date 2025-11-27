using Moq;
using Roborally.core.application.ApplicationContracts.Broadcasters;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.CommandContracts.Game;
using Roborally.core.application.CommandHandlers.Game;
using Roborally.core.domain;
using Roborally.core.domain.Bases;
using Roborally.unitTests.Factory;

namespace Roborally.unitTests.CommandHandlers.Game;

public class DealDecksToAllHandlerTests
{
    private readonly Mock<IGameRepository> _gameRepositoryMock;
    private readonly Mock<ISystemTime> _systemTimeMock;
    private readonly Mock<IIndividualPlayerBroadcaster> _individualPlayerBroadcasterMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly DealDecksToAllCommandHandler _handler;

    public DealDecksToAllHandlerTests()
    {
        _gameRepositoryMock = new Mock<IGameRepository>();
        _systemTimeMock = new Mock<ISystemTime>();
        _individualPlayerBroadcasterMock = new Mock<IIndividualPlayerBroadcaster>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _handler = new DealDecksToAllCommandHandler(_gameRepositoryMock.Object, _systemTimeMock.Object,
            _individualPlayerBroadcasterMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task CannotDealDecks_WhenGameDoesNotExist()
    {
        // Arrange
        _gameRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((core.domain.Game.Game?)null);

        var command = new DealDecksToAllCommand
        {
            GameId = Guid.NewGuid(),
            HostUsername = "HostPlayer"
        };

        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => _handler.ExecuteAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task DealDecks_WhenGameExists()
    {
        // Arrange
        var game = GameFactory.GetValidGame();
        _gameRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(game);

        var command = new DealDecksToAllCommand
        {
            GameId = game.GameId,
            HostUsername = "HostPlayer"
        };

        // Act
        await _handler.ExecuteAsync(command, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
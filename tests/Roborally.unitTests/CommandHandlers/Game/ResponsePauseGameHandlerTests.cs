using Moq;
using Roborally.core.application.ApplicationContracts.Broadcasters;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.CommandContracts.Game;
using Roborally.core.application.CommandHandlers.Game;
using Roborally.core.domain;
using Roborally.core.domain.Bases;
using Roborally.unitTests.Factory;

namespace Roborally.unitTests.CommandHandlers.Game;

public class ResponsePauseGameHandlerTests
{
    private readonly Mock<IGameRepository> _gameRepositoryMock;
    private readonly Mock<IGameBroadcaster> _gameBroadcasterMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ISystemTime> _systemTimeMock;
    private readonly ResponsePauseGameCommandHandler _handler;

    public ResponsePauseGameHandlerTests()
    {
        _gameRepositoryMock = new Mock<IGameRepository>();
        _gameBroadcasterMock = new Mock<IGameBroadcaster>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _systemTimeMock = new Mock<ISystemTime>();

        _handler = new ResponsePauseGameCommandHandler(_gameRepositoryMock.Object, _gameBroadcasterMock.Object,
            _unitOfWorkMock.Object, _systemTimeMock.Object);
    }

    [Fact]
    public async Task CannotRespondPauseGame_WhenGameDoesNotExist()
    {
        // Arrange
        _gameRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((core.domain.Game.Game?)null);

        var command = new ResponsePauseGameCommand
        {
            GameId = Guid.NewGuid(),
            ResponderUsername = "SomePlayer",
            Approved = true
        };

        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => _handler.ExecuteAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task RespondPauseGame_WhenGameExists_AndNotEveryoneResponded()
    {
        // Arrange
        var game = GameFactory.GetValidGame(playersCount: 3);
        game.RequestPauseGame(game.Players[0].Username, _systemTimeMock.Object);
        _gameRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(game);
        _systemTimeMock.Setup(st => st.CurrentTime).Returns(DateTime.UtcNow);

        var command = new ResponsePauseGameCommand
        {
            GameId = game.GameId,
            ResponderUsername = game.Players[1].Username,
            Approved = true
        };

        // Act
        await _handler.ExecuteAsync(command, CancellationToken.None);

        // Assert
        _gameBroadcasterMock.Verify(broadcaster =>
            broadcaster.BroadcastPauseGameRequestedAsync(game.GameId, command.ResponderUsername,
                It.IsAny<CancellationToken>()), Times.Once);
        _gameBroadcasterMock.Verify(broadcaster =>
            broadcaster.BroadcastPauseGameResultAsync(game.GameId, It.IsAny<core.domain.Game.GamePauseState>(),
                It.IsAny<CancellationToken>()), Times.Never);

        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RespondPauseGame_WhenGameExists_AndEveryoneResponded()
    {
        // Arrange
        var game = GameFactory.GetValidGame(playersCount: 2);
        game.RequestPauseGame(game.Players[0].Username, _systemTimeMock.Object);
        _gameRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(game);
        _systemTimeMock.Setup(st => st.CurrentTime).Returns(DateTime.UtcNow);

        var command = new ResponsePauseGameCommand
        {
            GameId = game.GameId,
            ResponderUsername = game.Players[1].Username,
            Approved = true
        };

        // Act
        await _handler.ExecuteAsync(command, CancellationToken.None);

        // Assert
        _gameBroadcasterMock.Verify(broadcaster =>
            broadcaster.BroadcastPauseGameRequestedAsync(game.GameId, command.ResponderUsername,
                It.IsAny<CancellationToken>()), Times.Once);
        _gameBroadcasterMock.Verify(broadcaster =>
            broadcaster.BroadcastPauseGameResultAsync(game.GameId, It.IsAny<core.domain.Game.GamePauseState>(),
                It.IsAny<CancellationToken>()), Times.Once);

        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
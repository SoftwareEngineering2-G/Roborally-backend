using Moq;
using Roborally.core.domain;
using Roborally.core.domain.Game;
using Roborally.core.domain.Game.CardActions;
using Roborally.core.domain.Game.Gameboard.Space;
using Roborally.core.domain.Game.Player;
using Roborally.core.domain.Game.Deck;
using Roborally.core.domain.Bases;
using Roborally.unitTests.Factory;

namespace Roborally.unitTests.Domain;

public class Move3CardActionTests
{
    private readonly Game _game;
    private readonly Player _player;
    private readonly Move3CardAction _action;
    private readonly Mock<ISystemTime> _systemTimeMock;

    public Move3CardActionTests()
    {
        _game = GameFactory.GetValidGame();
        _player = _game.Players[0];
        _action = new Move3CardAction();
        _systemTimeMock = new Mock<ISystemTime>();
        _systemTimeMock.Setup(x => x.CurrentTime).Returns(DateTime.UtcNow);
    }

    [Fact]
    public void Move3_ShouldMovePlayerThreeSpacesForward()
    {
        // Arrange
        _player.CurrentPosition = new Position(5, 5);
        _player.CurrentFacingDirection = Direction.North;

        // Act
        _action.Execute(_player, _game, _systemTimeMock.Object);

        // Assert
        Assert.Equal(new Position(5, 2), _player.CurrentPosition);
    }
    
    [Fact]
    public void Move3_ShouldRecordCardExecution()
    {
        // Arrange
        _player.CurrentPosition = new Position(5, 5);
        _player.CurrentFacingDirection = Direction.North;

        // Act
        _action.Execute(_player, _game, _systemTimeMock.Object);

        // Assert
        var lastCard = _player.GetLastExecutedCard();
        Assert.Equal(ProgrammingCard.Move3, lastCard);
    }
}

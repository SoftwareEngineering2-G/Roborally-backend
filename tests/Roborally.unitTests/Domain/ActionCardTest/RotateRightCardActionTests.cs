using Moq;
using Roborally.core.domain.Game;
using Roborally.core.domain.Game.CardActions;
using Roborally.core.domain.Game.Player;
using Roborally.core.domain.Game.Deck;
using Roborally.core.domain.Bases;
using Roborally.unitTests.Factory;

namespace Roborally.unitTests.Domain;

public class RotateRightCardActionTests
{
    private readonly Game _game;
    private readonly Player _player;
    private readonly RotateRightCardAction _action;
    private readonly Mock<ISystemTime> _systemTimeMock;

    public RotateRightCardActionTests()
    {
        _game = GameFactory.GetValidGame();
        _player = _game.Players[0];
        _action = new RotateRightCardAction();
        _systemTimeMock = new Mock<ISystemTime>();
        _systemTimeMock.Setup(x => x.CurrentTime).Returns(DateTime.UtcNow);
    }

    [Fact]
    public void RotateRight_ShouldRotateFromNorthToEast()
    {
        // Arrange
        _player.CurrentPosition = new Position(5, 5);
        _player.CurrentFacingDirection = Direction.North;

        // Act
        _action.Execute(_player, _game, _systemTimeMock.Object);

        // Assert
        Assert.Equal(Direction.East, _player.CurrentFacingDirection);
    }

    [Fact]
    public void RotateRight_ShouldRotateFromEastToSouth()
    {
        // Arrange
        _player.CurrentPosition = new Position(5, 5);
        _player.CurrentFacingDirection = Direction.East;

        // Act
        _action.Execute(_player, _game, _systemTimeMock.Object);

        // Assert
        Assert.Equal(Direction.South, _player.CurrentFacingDirection);
    }

    [Fact]
    public void RotateRight_ShouldRotateFromSouthToWest()
    {
        // Arrange
        _player.CurrentPosition = new Position(5, 5);
        _player.CurrentFacingDirection = Direction.South;

        // Act
        _action.Execute(_player, _game, _systemTimeMock.Object);

        // Assert
        Assert.Equal(Direction.West, _player.CurrentFacingDirection);
    }

    [Fact]
    public void RotateRight_ShouldRotateFromWestToNorth()
    {
        // Arrange
        _player.CurrentPosition = new Position(5, 5);
        _player.CurrentFacingDirection = Direction.West;

        // Act
        _action.Execute(_player, _game, _systemTimeMock.Object);

        // Assert
        Assert.Equal(Direction.North, _player.CurrentFacingDirection);
    }

    [Fact]
    public void RotateRight_ShouldNotChangePosition()
    {
        // Arrange
        var startPosition = new Position(5, 5);
        _player.CurrentPosition = startPosition;
        _player.CurrentFacingDirection = Direction.North;

        // Act
        _action.Execute(_player, _game, _systemTimeMock.Object);

        // Assert
        Assert.Equal(startPosition, _player.CurrentPosition);
    }

    [Fact]
    public void RotateRight_ShouldRecordCardExecution()
    {
        // Arrange
        _player.CurrentPosition = new Position(5, 5);
        _player.CurrentFacingDirection = Direction.North;

        // Act
        _action.Execute(_player, _game, _systemTimeMock.Object);

        // Assert
        var lastCard = _player.GetLastExecutedCard();
        Assert.Equal(ProgrammingCard.RotateRight, lastCard);
    }

    [Fact]
    public void RotateRight_ShouldCompleteFullCircleAfterFourRotations()
    {
        // Arrange
        _player.CurrentPosition = new Position(5, 5);
        _player.CurrentFacingDirection = Direction.North;
        var startDirection = _player.CurrentFacingDirection;

        // Act - Rotate 4 times
        _action.Execute(_player, _game, _systemTimeMock.Object);
        _action.Execute(_player, _game, _systemTimeMock.Object);
        _action.Execute(_player, _game, _systemTimeMock.Object);
        _action.Execute(_player, _game, _systemTimeMock.Object);

        // Assert
        Assert.Equal(startDirection, _player.CurrentFacingDirection);
    }

    [Fact]
    public void RotateRight_ShouldWorkFromAnyPosition()
    {
        // Arrange
        _player.CurrentPosition = new Position(0, 0);
        _player.CurrentFacingDirection = Direction.East;

        // Act
        _action.Execute(_player, _game, _systemTimeMock.Object);

        // Assert
        Assert.Equal(Direction.South, _player.CurrentFacingDirection);
        Assert.Equal(new Position(0, 0), _player.CurrentPosition);
    }

    [Fact]
    public void RotateRight_ShouldWorkAtBoardEdge()
    {
        // Arrange
        _player.CurrentPosition = new Position(9, 9);
        _player.CurrentFacingDirection = Direction.West;

        // Act
        _action.Execute(_player, _game, _systemTimeMock.Object);

        // Assert
        Assert.Equal(Direction.North, _player.CurrentFacingDirection);
        Assert.Equal(new Position(9, 9), _player.CurrentPosition);
    }
}

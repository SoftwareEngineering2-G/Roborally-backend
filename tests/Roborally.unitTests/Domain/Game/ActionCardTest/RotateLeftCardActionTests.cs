using Moq;
using Roborally.core.domain.Game;
using Roborally.core.domain.Game.CardActions;
using Roborally.core.domain.Game.Player;
using Roborally.core.domain.Game.Deck;
using Roborally.core.domain.Bases;
using Roborally.unitTests.Factory;

namespace Roborally.unitTests.Domain;

public class RotateLeftCardActionTests
{
    private readonly core.domain.Game.Game _game;
    private readonly Player _player;
    private readonly RotateLeftCardAction _action;
    private readonly Mock<ISystemTime> _systemTimeMock;

    public RotateLeftCardActionTests()
    {
        _game = GameFactory.GetValidGame();
        _player = _game.Players[0];
        _action = new RotateLeftCardAction();
        _systemTimeMock = new Mock<ISystemTime>();
        _systemTimeMock.Setup(x => x.CurrentTime).Returns(DateTime.UtcNow);
    }

    [Fact]
    public void RotateLeft_ShouldRotateFromNorthToWest()
    {
        // Arrange
        _player.CurrentPosition = new Position(5, 5);
        _player.CurrentFacingDirection = Direction.North;

        // Act
        _action.Execute(_player, _game, _systemTimeMock.Object);

        // Assert
        Assert.Equal(Direction.West, _player.CurrentFacingDirection);
    }

    [Fact]
    public void RotateLeft_ShouldRotateFromWestToSouth()
    {
        // Arrange
        _player.CurrentPosition = new Position(5, 5);
        _player.CurrentFacingDirection = Direction.West;

        // Act
        _action.Execute(_player, _game, _systemTimeMock.Object);

        // Assert
        Assert.Equal(Direction.South, _player.CurrentFacingDirection);
    }

    [Fact]
    public void RotateLeft_ShouldRotateFromSouthToEast()
    {
        // Arrange
        _player.CurrentPosition = new Position(5, 5);
        _player.CurrentFacingDirection = Direction.South;

        // Act
        _action.Execute(_player, _game, _systemTimeMock.Object);

        // Assert
        Assert.Equal(Direction.East, _player.CurrentFacingDirection);
    }

    [Fact]
    public void RotateLeft_ShouldRotateFromEastToNorth()
    {
        // Arrange
        _player.CurrentPosition = new Position(5, 5);
        _player.CurrentFacingDirection = Direction.East;

        // Act
        _action.Execute(_player, _game, _systemTimeMock.Object);

        // Assert
        Assert.Equal(Direction.North, _player.CurrentFacingDirection);
    }

    [Fact]
    public void RotateLeft_ShouldNotChangePosition()
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
    public void RotateLeft_ShouldRecordCardExecution()
    {
        // Arrange
        _player.CurrentPosition = new Position(5, 5);
        _player.CurrentFacingDirection = Direction.North;

        // Act
        _action.Execute(_player, _game, _systemTimeMock.Object);

        // Assert
        var lastCard = _player.GetLastExecutedCard();
        Assert.Equal(ProgrammingCard.RotateLeft, lastCard);
    }
}

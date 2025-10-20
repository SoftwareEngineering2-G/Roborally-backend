using Moq;
using Roborally.core.domain.Game;
using Roborally.core.domain.Game.CardActions;
using Roborally.core.domain.Game.Player;
using Roborally.core.domain.Game.Deck;
using Roborally.core.domain.Bases;
using Roborally.unitTests.Factory;

namespace Roborally.unitTests.Domain;

public class UTurnCardActionTests
{
    private readonly Game _game;
    private readonly Player _player;
    private readonly UTurnCardAction _action;
    private readonly Mock<ISystemTime> _systemTimeMock;

    public UTurnCardActionTests()
    {
        _game = GameFactory.GetValidGame();
        _player = _game.Players[0];
        _action = new UTurnCardAction();
        _systemTimeMock = new Mock<ISystemTime>();
        _systemTimeMock.Setup(x => x.CurrentTime).Returns(DateTime.UtcNow);
    }

    [Fact]
    public void UTurn_ShouldRotateFromNorthToSouth()
    {
        // Arrange
        _player.CurrentPosition = new Position(5, 5);
        _player.CurrentFacingDirection = Direction.North;

        // Act
        _action.Execute(_player, _game, _systemTimeMock.Object);

        // Assert
        Assert.Equal(Direction.South, _player.CurrentFacingDirection);
    }

    [Fact]
    public void UTurn_ShouldRotateFromEastToWest()
    {
        // Arrange
        _player.CurrentPosition = new Position(5, 5);
        _player.CurrentFacingDirection = Direction.East;

        // Act
        _action.Execute(_player, _game, _systemTimeMock.Object);

        // Assert
        Assert.Equal(Direction.West, _player.CurrentFacingDirection);
    }

    [Fact]
    public void UTurn_ShouldRotateFromSouthToNorth()
    {
        // Arrange
        _player.CurrentPosition = new Position(5, 5);
        _player.CurrentFacingDirection = Direction.South;

        // Act
        _action.Execute(_player, _game, _systemTimeMock.Object);

        // Assert
        Assert.Equal(Direction.North, _player.CurrentFacingDirection);
    }

    [Fact]
    public void UTurn_ShouldRotateFromWestToEast()
    {
        // Arrange
        _player.CurrentPosition = new Position(5, 5);
        _player.CurrentFacingDirection = Direction.West;

        // Act
        _action.Execute(_player, _game, _systemTimeMock.Object);

        // Assert
        Assert.Equal(Direction.East, _player.CurrentFacingDirection);
    }

    [Fact]
    public void UTurn_ShouldNotChangePosition()
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
    public void UTurn_ShouldRecordCardExecution()
    {
        // Arrange
        _player.CurrentPosition = new Position(5, 5);
        _player.CurrentFacingDirection = Direction.North;

        // Act
        _action.Execute(_player, _game, _systemTimeMock.Object);

        // Assert
        var lastCard = _player.GetLastExecutedCard();
        Assert.Equal(ProgrammingCard.UTurn, lastCard);
    }
}

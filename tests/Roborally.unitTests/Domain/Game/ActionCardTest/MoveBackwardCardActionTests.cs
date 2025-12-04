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

public class MoveBackwardCardActionTests
{
    private readonly core.domain.Game.Game _game;
    private readonly Player _player;
    private readonly MoveBackwardCardAction _action;
    private readonly Mock<ISystemTime> _systemTimeMock;

    public MoveBackwardCardActionTests()
    {
        _game = GameFactory.GetValidGame();
        _player = _game.Players[0];
        _action = new MoveBackwardCardAction(1); // Default to 1 space backward
        _systemTimeMock = new Mock<ISystemTime>();
        _systemTimeMock.Setup(x => x.CurrentTime).Returns(DateTime.UtcNow);
    }

    [Fact]
    public void MoveBackward_ShouldMovePlayerBackwardOneSpace()
    {
        // Arrange
        _player.CurrentPosition = new Position(5, 5);
        _player.CurrentFacingDirection = Direction.North;

        // Act
        _action.Execute(_player, _game, _systemTimeMock.Object);

        // Assert - Should move South (opposite of North)
        Assert.Equal(new Position(5, 6), _player.CurrentPosition);
    }

    [Fact]
    public void MoveBackward_ShouldMoveBackwardWhenFacingEast()
    {
        // Arrange
        _player.CurrentPosition = new Position(5, 5);
        _player.CurrentFacingDirection = Direction.East;

        // Act
        _action.Execute(_player, _game, _systemTimeMock.Object);

        // Assert - Should move West (opposite of East)
        Assert.Equal(new Position(4, 5), _player.CurrentPosition);
    }

    [Fact]
    public void MoveBackward_ShouldMoveBackwardWhenFacingSouth()
    {
        // Arrange
        _player.CurrentPosition = new Position(5, 5);
        _player.CurrentFacingDirection = Direction.South;

        // Act
        _action.Execute(_player, _game, _systemTimeMock.Object);

        // Assert - Should move North (opposite of South)
        Assert.Equal(new Position(5, 4), _player.CurrentPosition);
    }

    [Fact]
    public void MoveBackward_ShouldMoveBackwardWhenFacingWest()
    {
        // Arrange
        _player.CurrentPosition = new Position(5, 5);
        _player.CurrentFacingDirection = Direction.West;

        // Act
        _action.Execute(_player, _game, _systemTimeMock.Object);

        // Assert - Should move East (opposite of West)
        Assert.Equal(new Position(6, 5), _player.CurrentPosition);
    }

    [Fact]
    public void MoveBackward_ShouldPushRobotBackward()
    {
        // Arrange
        var player2 = _game.Players[1];
        _player.CurrentPosition = new Position(5, 5);
        _player.CurrentFacingDirection = Direction.North; // Backward = South
        player2.CurrentPosition = new Position(5, 6); // One space south

        // Act
        _action.Execute(_player, _game, _systemTimeMock.Object);

        // Assert
        Assert.Equal(new Position(5, 6), _player.CurrentPosition); // Moved backward
        Assert.Equal(new Position(5, 7), player2.CurrentPosition); // Pushed backward
    }

    [Fact]
    public void MoveBackward_ShouldRecordCardExecution()
    {
        // Arrange
        _player.CurrentPosition = new Position(5, 5);
        _player.CurrentFacingDirection = Direction.North;

        // Act
        _action.Execute(_player, _game, _systemTimeMock.Object);

        // Assert
        var lastCard = _player.GetLastExecutedCard();
        Assert.Equal(ProgrammingCard.MoveBack, lastCard);
    }
}

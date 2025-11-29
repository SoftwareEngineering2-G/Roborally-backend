using Roborally.core.domain.Game;
using Roborally.core.domain.Game.Gameboard.Space;
using Roborally.core.domain.Game.Player;
using Roborally.unitTests.Factory;

namespace Roborally.unitTests.Domain;

public class GameMovementTests
{
    private readonly core.domain.Game.Game _game;
    private readonly Player _player;

    public GameMovementTests()
    {
        _game = GameFactory.GetValidGame();
        _player = _game.Players[0];
        _player.CurrentPosition = new Position(0, 0);
        _player.CurrentFacingDirection = Direction.East;
    }

    [Fact]
    public void RobotShouldNotMove_WhenFacingWall_OnSameSpace()
    {
        // Arrange
        var playerStartingPosition = _player.CurrentPosition;
        var playerFacingDirection = _player.CurrentFacingDirection;

        _game.GameBoard.Spaces[playerStartingPosition.Y][playerStartingPosition.X] =
            new EmptySpace([playerFacingDirection]);

        // Act
        _game.MovePlayerInDirection(_player, playerFacingDirection);

        // Assert
        Assert.Equal(playerStartingPosition, _player.CurrentPosition);
    }

    [Fact]
    public void RobotShouldNotMove_WhenFacingWall_OnAdjacentSpace()
    {
        // Arrange
        var playerStartingPosition = _player.CurrentPosition;
        var playerFacingDirection = _player.CurrentFacingDirection;

        _game.GameBoard.Spaces[playerStartingPosition.Y][playerStartingPosition.X + 1] =
            new EmptySpace([playerFacingDirection.Opposite()]);
        
        // Act
        _game.MovePlayerInDirection(_player, playerFacingDirection);
        
        // Assert
        Assert.Equal(playerStartingPosition, _player.CurrentPosition);
    }
}
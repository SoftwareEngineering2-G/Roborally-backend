using Roborally.core.domain.Game.Gameboard;
using Roborally.core.domain.Game.Gameboard.Space;
using Roborally.core.domain.Game.Player;

namespace Roborally.unitTests.Domain;

public class GameBoardTests
{
    private readonly GameBoard _gameBoard;
    
    public GameBoardTests()
    {
        _gameBoard = BoardFactory.GetEmptyBoard();
    }

    [Fact]
    public void HasWallsBetween_RecognizesWallOnFromSpace()
    {
        // Arrange
        var from = new Position(0, 0);
        var to = new Position(1, 0);
        var direction = Direction.East;
        _gameBoard.Spaces[from.Y][from.X] = new EmptySpace([direction]);
        
        // Act
        var hasWallBetween = _gameBoard.HasWallBetween(from, to, direction);
        
        // Assert
        Assert.True(hasWallBetween);
    }
    
    [Fact]
    public void HasWallsBetween_RecognizesWallOnToSpace()
    {
        // Arrange
        var from = new Position(0, 0);
        var to = new Position(1, 0);
        var direction = Direction.East;
        _gameBoard.Spaces[to.Y][to.X] = new EmptySpace([direction]);
        
        // Act
        var hasWallBetween = _gameBoard.HasWallBetween(from, to, direction);
        
        // Assert
        Assert.True(hasWallBetween);
    }
}
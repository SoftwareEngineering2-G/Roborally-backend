using Roborally.core.domain.Bases;
using Roborally.core.domain.Game.Player;

namespace Roborally.core.domain.Game.Gameboard;

public class GameBoard {
    public required string Name { get; set; }
    public required Space.Space[][] Space { get; init; }

    internal GameBoard() {

    }
    
    public bool IsWithinBounds(Position position) {
        if (position.Y < 0 || position.Y >= Space.Length) 
            return false;
        if (position.X < 0 || position.X >= Space[position.Y].Length) 
            return false;
        return true;
    }

    public bool HasWallBetween(Position from, Position to, Direction direction) {
        // Check adjacency in the specified direction
        Position expectedTo = direction.GetNextPosition(from);
        if (!expectedTo.Equals(to))
            return false;

        Space.Space fromSpace = Space[from.Y][from.X];
        Space.Space toSpace = Space[to.Y][to.X];
        // Check for wall in the given direction from 'from', or in the opposite direction from 'to'
        if (fromSpace.Walls().Contains(direction))
            return true;
        if (toSpace.Walls().Contains(direction.Opposite()))
            return true;
        return false;
    }
    
    public Space.Space GetSpaceAt(int x, int y) {
        if (y < 0 || y >= Space.Length || x < 0 || x >= Space[0].Length)
            throw new ArgumentOutOfRangeException();
        return Space[y][x];
    }
}
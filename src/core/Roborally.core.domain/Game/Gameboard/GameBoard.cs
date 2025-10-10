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
        Space.Space fromSpace = Space[from.Y][from.X];
        return fromSpace.Walls().Contains(direction);
    }
}
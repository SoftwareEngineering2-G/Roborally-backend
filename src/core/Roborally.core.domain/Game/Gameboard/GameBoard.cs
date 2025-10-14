using Roborally.core.domain.Game.Player;

namespace Roborally.core.domain.Game.Gameboard;

public class GameBoard {
    public required string Name { get; set; }
    public required Space.Space[][] Spaces { get; init; }

    internal GameBoard() {

    }
    
    public bool IsWithinBounds(Position position) {
        if (position.Y < 0 || position.Y >= Spaces.Length) 
            return false;
        if (position.X < 0 || position.X >= Spaces[position.Y].Length) 
            return false;
        return true;
    }

    public bool HasWallBetween(Position from, Position to, Direction direction) {
        // Check adjacency in the specified direction
        Position expectedTo = direction.GetNextPosition(from);
        if (!expectedTo.Equals(to))
            return false;

        Space.Space fromSpace = Spaces[from.Y][from.X];
        Space.Space toSpace = Spaces[to.Y][to.X];
        // Check for wall in the given direction from 'from', or in the opposite direction from 'to'
        if (fromSpace.Walls().Contains(direction))
            return true;
        if (toSpace.Walls().Contains(direction.Opposite()))
            return true;
        return false;
    }

    
    public Space.Space GetSpaceAt(int x, int y) {
        if (y < 0 || y >= Spaces.Length || x < 0 || x >= Spaces[0].Length)
            throw new ArgumentOutOfRangeException();
        return Spaces[y][x];
    }


    public List<T> GetAllSpacesOfType<T>() where T : Space.Space {
        var result = new List<T>();
        foreach (var row in Spaces) {
            foreach (var space in row) {
                if (space is T typedSpace) {
                    result.Add(typedSpace);
                }
            }
        }
        return result;
    }

    public IList<BoardElement.BoardElement> GetElementsForActivationOfType(string nextBoardElement) {
        var result = new List<BoardElement.BoardElement>();
        foreach (var row in Spaces) {
            foreach (var space in row) {
                if (space is BoardElement.BoardElement boardElement && boardElement.Name().Equals(nextBoardElement)) {
                    result.Add(boardElement);
                }
            }
        }
        return result;
    }
}
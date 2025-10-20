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

    
    public Space.Space GetSpaceAt(Position position) {
        if (position.Y < 0 || position.Y>= Spaces.Length || position.X < 0 || position.X >= Spaces[0].Length)
            throw new ArgumentOutOfRangeException();
        return Spaces[position.Y][position.X];
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

    public Dictionary<Player.Player, BoardElement.BoardElement> FilterPlayersOnBoardElements(List<Player.Player> players, string boardElementName) {
        var filtered = new Dictionary<Player.Player, BoardElement.BoardElement>();
        foreach (var player in players) {
            var space = GetSpaceAt(player.CurrentPosition);
            if (space is BoardElement.BoardElement element && element.Name().Equals(boardElementName)) {
                filtered[player] = element;
            }
        }
        return filtered;
    }

    public List<Position> GetPositionsForSpaceType(string spawnPointName) {
        var positions = new List<Position>();
        for (int y = 0; y < Spaces.Length; y++) {
            for (int x = 0; x < Spaces[y].Length; x++) {
                if (Spaces[y][x].Name().Equals(spawnPointName)) {
                    positions.Add(new Position(x, y));
                }
            }
        }
        return positions;
    }
}
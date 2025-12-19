using Roborally.core.domain.Game.Player;

namespace Roborally.core.domain.Game.Gameboard;

public class GameBoard {
    public required string Name { get; set; }
    public required Space.Space[][] Spaces { get; init; }

/// <author name="Vincenzo Altaserse 2025-10-13 13:44:06 +0200 9" />
    internal GameBoard() {

    }
    
/// <author name="Suhani Pandey 2025-10-10 13:01:53 +0200 13" />
    public bool IsWithinBounds(Position position) {
        if (position.Y < 0 || position.Y >= Spaces.Length) 
            return false;
        if (position.X < 0 || position.X >= Spaces[position.Y].Length) 
            return false;
        return true;
    }

/// <author name="Suhani Pandey 2025-10-10 13:01:53 +0200 21" />
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

    
/// <author name="Nilanjana Devkota 2025-10-14 19:37:00 +0200 38" />
    public Space.Space GetSpaceAt(Position position) {
        if (position.Y < 0 || position.Y>= Spaces.Length || position.X < 0 || position.X >= Spaces[0].Length)
            throw new ArgumentOutOfRangeException();
        return Spaces[position.Y][position.X];
    }


/// <author name="Nilanjana Devkota 2025-10-14 19:37:00 +0200 45" />
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

/// <author name="Nilanjana Devkota 2025-10-14 19:37:00 +0200 57" />
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

/// <author name="Sachin Baral 2025-10-20 21:20:17 +0200 68" />
    public List<Position> GetPositionsForSpaceType(string spaceType) {
        var positions = new List<Position>();
        for (int y = 0; y < Spaces.Length; y++) {
            for (int x = 0; x < Spaces[y].Length; x++) {
                if (Spaces[y][x].Name().Equals(spaceType)) {
                    positions.Add(new Position(x, y));
                }
            }
        }
        return positions;
    }
}
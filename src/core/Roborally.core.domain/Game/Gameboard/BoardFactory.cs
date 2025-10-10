using Roborally.core.domain.Game.Player;

namespace Roborally.core.domain.Game.Gameboard;

using Space;

public static class BoardFactory {
    private static readonly Lock Lock = new ();
    private static GameBoard? _emptyBoard;
    private static GameBoard? _boardWithWalls;

    public static GameBoard GetEmptyBoard() {
        if (_emptyBoard is not null) {
            return _emptyBoard;
        }

        lock (Lock) {
            // Double-check locking pattern to ensure thread safety
            if (_emptyBoard is not null) {
                return _emptyBoard;
            }

            Space.Space[][] spaces = new Space.Space[10][];
            
            for (int i = 0; i < 10; i++) {
                spaces[i] = new Space.Space[10];
                for (int j = 0; j < 10; j++) {
                    spaces[i][j] = new EmptySpace([Direction.East]);
                }
            }

            _emptyBoard = new GameBoard() {
                Name = "Empty Board",
                Space = spaces
            };
        }

        return _emptyBoard;
    }
    
    public static GameBoard GetBoardWithWalls() {
        if (_boardWithWalls is not null) {
            return _boardWithWalls;
        }
        
        lock (Lock) {
            // Double-check locking pattern to ensure thread safety
            if (_boardWithWalls is not null) {
                return _boardWithWalls;
            }
            
            const int boardSize = 10;
            Space.Space[][] spaces = new Space.Space[boardSize][];
            Random random = new Random();
    
            for (int i = 0; i < boardSize; i++) {
                spaces[i] = new Space.Space[boardSize];
                for (int j = 0; j < boardSize; j++) {
                    List<Direction> walls = new List<Direction>();
    
                    // Check left cell (West direction)
                    if (j > 0 && spaces[i][j - 1].Walls().Contains(Direction.East)) {
                        walls.Add(Direction.West);
                    }
    
                    // Check top cell (North direction)
                    if (i > 0 && spaces[i - 1][j].Walls().Contains(Direction.South)) {
                        walls.Add(Direction.North);
                    }
    
                    // Decide how many additional walls to create (0, 1, 2, or 3)
                    // Determine number of walls based on weighted probability
                    int rand = random.Next(100);
                    int totalWalls = rand < 50 ? 0 : rand < 75 ? 1 : rand < 95 ? 2 : 3;
                    int additionalWalls = totalWalls - walls.Count;
                    
                    // Get available directions (not already used and not on outer borders)
                    List<Direction> availableDirections = new List<Direction> 
                        { Direction.South, Direction.East };
                    
                    // Remove directions that would place walls outside the board
                    if (i == boardSize - 1) availableDirections.Remove(Direction.South);
                    if (j == boardSize - 1) availableDirections.Remove(Direction.East);
    
                    // Add random walls
                    for (int w = 0; w < additionalWalls && availableDirections.Count > 0; w++) {
                        int index = random.Next(availableDirections.Count);
                        walls.Add(availableDirections[index]);
                        availableDirections.RemoveAt(index);
                    }
    
                    spaces[i][j] = new EmptySpace(walls.ToArray());
                }
            }
    
            _boardWithWalls = new GameBoard() {
                Name = "Board With Walls",
                Space = spaces
            };
            
        }
        
        return _boardWithWalls;
    }
    
}
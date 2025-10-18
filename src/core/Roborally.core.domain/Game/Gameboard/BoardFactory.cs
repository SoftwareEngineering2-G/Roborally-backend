using Roborally.core.domain.Game.Player;

namespace Roborally.core.domain.Game.Gameboard;

using Space;
using BoardElement; // allow referring to BoardElementFactory and types

public static class BoardFactory {
    private static readonly Lock Lock = new ();
    private static GameBoard? _emptyBoard;
    private static GameBoard? _boardWithWalls;
    
    private const int BoardWidth = 15;
    private const int BoardHeight = 12;

    public static GameBoard GetEmptyBoard() {
        if (_emptyBoard is not null) {
            return _emptyBoard;
        }

        lock (Lock) {
            // Double-check locking pattern to ensure thread safety
            if (_emptyBoard is not null) {
                return _emptyBoard;
            }

            Space.Space[][] spaces = new Space.Space[BoardHeight][];
            
            for (int i = 0; i < BoardHeight; i++) {
                spaces[i] = new Space.Space[BoardWidth];
                for (int j = 0; j < BoardWidth; j++) {
                    spaces[i][j] = new EmptySpace([Direction.East]);
                }
            }

            _emptyBoard = new GameBoard() {
                Name = "Empty Board",
                Spaces = spaces
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
            
            Space.Space[][] spaces = new Space.Space[BoardHeight][];
            Random random = new Random();
    
            for (int i = 0; i < BoardHeight; i++) {
                spaces[i] = new Space.Space[BoardWidth];
                for (int j = 0; j < BoardWidth; j++) {
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
                    if (i == BoardHeight - 1) availableDirections.Remove(Direction.South);
                    if (j == BoardWidth - 1) availableDirections.Remove(Direction.East);
    
                    // Add random walls
                    for (int w = 0; w < additionalWalls && availableDirections.Count > 0; w++) {
                        int index = random.Next(availableDirections.Count);
                        walls.Add(availableDirections[index]);
                        availableDirections.RemoveAt(index);
                    }
    
                    // Decide whether to place a board element (conveyor belt or gear)
                    // Assumption: small per-cell probabilities. Adjust as needed later.
                    // - 6% Blue Conveyor
                    // - 6% Green Conveyor
                    // - 4% Gear
                    int placeRoll = random.Next(100);

                    Space.Space createdSpace;

                    if (placeRoll < 6) {
                        // Blue conveyor with random direction
                        Direction[] dirs = { Direction.North, Direction.East, Direction.South, Direction.West };
                        Direction dir = dirs[random.Next(dirs.Length)];
                        createdSpace = BoardElementFactory.BlueConveyorBelt(dir, walls.ToArray());
                    } else if (placeRoll < 12) {
                        // Green conveyor with random direction
                        Direction[] dirs = { Direction.North, Direction.East, Direction.South, Direction.West };
                        Direction dir = dirs[random.Next(dirs.Length)];
                        createdSpace = BoardElementFactory.GreenConveyorBelt(dir, walls.ToArray());
                    } else if (placeRoll < 16) {
                        // Gear with random rotation
                        var gearDir = (random.Next(2) == 0) ? GearDirection.ClockWise : GearDirection.AntiClockWise;
                        createdSpace = BoardElementFactory.Gear(gearDir, walls.ToArray());
                    } else {
                        // Default: empty space with computed walls
                        createdSpace = new EmptySpace(walls.ToArray());
                    }
    
                    spaces[i][j] = createdSpace;
                }
            }
    
            _boardWithWalls = new GameBoard() {
                Name = "Board With Walls",
                Spaces = spaces
            };
            
        }
        
        return _boardWithWalls;
    }
    
}
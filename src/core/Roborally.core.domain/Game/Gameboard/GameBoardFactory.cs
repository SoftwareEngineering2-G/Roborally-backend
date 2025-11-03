using Roborally.core.domain.Game.Player;

namespace Roborally.core.domain.Game.Gameboard;

using Space;
using BoardElement; // allow referring to BoardElementFactory and types

public static class GameBoardFactory {
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
    public static GameBoard GetStarterCourse()
    {
        // Create a 15x12 board initialized with empty spaces
        Space.Space[][] spaces = new Space.Space[BoardHeight][];
        for (int row = 0; row < BoardHeight; row++)
        {
            spaces[row] = new Space.Space[BoardWidth];
            for (int col = 0; col < BoardWidth; col++)
            {
                spaces[row][col] = new EmptySpace(Array.Empty<Direction>());
            }
        }

        // Helper methods for placing conveyor belts
        void PlaceBlueConveyor(int row, int col, Direction direction, Direction[]? walls = null) =>
            spaces[row][col] = BoardElementFactory.BlueConveyorBelt(direction, walls);

        void PlaceGreenConveyor(int row, int col, Direction direction, Direction[]? walls = null) =>
            spaces[row][col] = BoardElementFactory.GreenConveyorBelt(direction, walls);
        
        void PlaceSpawnPoint(int row, int col, Direction[]? walls = null) =>
            spaces[row][col] = SpaceFactory.SpawnPoint(walls);

        // ========== Blue Conveyor Belt Circuits ==========
        
        // Blue Circuit 1: Left side vertical loop
        // Vertical line down (row 0-5, col 1)
        for (int row = 0; row <= 5; row++)
        {
            PlaceBlueConveyor(row, 1, Direction.South);
        }
        // Horizontal connector (row 5, col 2)
        PlaceBlueConveyor(5, 2, Direction.East);
        // Vertical line up (row 5-0, col 3)
        for (int row = 5; row >= 0; row--)
        {
            PlaceBlueConveyor(row, 3, Direction.North);
        }

        // Blue Circuit 2: Right side vertical loop
        // Horizontal line left (row 1, col 11-6)
        for (int col = 11; col >= 6; col--)
        {
            PlaceBlueConveyor(1, col, Direction.West);
        }
        // Vertical connector (row 2, col 6)
        PlaceBlueConveyor(1, 6, Direction.South);

        PlaceBlueConveyor(2, 6, Direction.South);
        // Horizontal line right (row 3, col 6-11)
        for (int col = 6; col <= 11; col++)
        {
            PlaceBlueConveyor(3, col, Direction.East);
        }

        // Blue Circuit 3: Bottom left L-shape
        // Vertical line down (row 8, col 0-5)
        for (int col = 0; col <= 5; col++)
        {
            PlaceBlueConveyor(8, col, Direction.West);
        }
        // Corner pieces (row 9-10, col 5)
        PlaceBlueConveyor(9, 5, Direction.North);
        PlaceBlueConveyor(10, 5, Direction.West);
        // Horizontal line right (row 10, col 0-4)
        for (int col = 0; col <= 4; col++)
        {
            PlaceBlueConveyor(10, col, Direction.East);
        }

        // Blue Circuit 4: Bottom right U-shape
        // Vertical line down (row 6-11, col 8)
        for (int row = 6; row <= 11; row++)
        {
            PlaceBlueConveyor(row, 8, Direction.South);
        }
        // Horizontal connectors (row 6, col 9-10)
        PlaceBlueConveyor(6, 9, Direction.East);
        PlaceBlueConveyor(6, 10, Direction.North);
        // Vertical line up (row 7-11, col 10)
        for (int row = 7; row <= 11; row++)
        {
            PlaceBlueConveyor(row, 10, Direction.North);
        }

        // ========== Green Conveyor Belt Circuits ==========
        
        // Green Circuit 1: Complex left spiral
        // Vertical down (row 0-4, col 5)
        for (int row = 0; row <= 4; row++)
        {
            PlaceGreenConveyor(row, 5, Direction.South);
        }
        
        // Spiral pattern
        PlaceGreenConveyor(5, 5, Direction.West);
        PlaceGreenConveyor(5, 4, Direction.West);
        PlaceGreenConveyor(6, 4, Direction.West);
        PlaceGreenConveyor(6, 3, Direction.West);
        PlaceGreenConveyor(7, 3, Direction.West);
        PlaceGreenConveyor(7, 2, Direction.North);
        PlaceGreenConveyor(6, 2, Direction.West);
        PlaceGreenConveyor(6, 1, Direction.West);
        PlaceGreenConveyor(6, 0, Direction.North);
        PlaceGreenConveyor(5, 0, Direction.North);

        // Green Circuit 2: Complex right circuit
        PlaceGreenConveyor(5, 11, Direction.West);
        PlaceGreenConveyor(4, 11, Direction.West);
        PlaceGreenConveyor(4, 10, Direction.West);
        PlaceGreenConveyor(4, 9, Direction.South);
        PlaceGreenConveyor(4, 8, Direction.South);
        PlaceGreenConveyor(5, 7, Direction.South);
        PlaceGreenConveyor(6, 7, Direction.South, [Direction.East]);
        PlaceGreenConveyor(7, 7, Direction.South);
        PlaceGreenConveyor(8, 7, Direction.West);
        PlaceGreenConveyor(8, 6, Direction.South);
        PlaceGreenConveyor(9, 6, Direction.South);
        PlaceGreenConveyor(10, 6, Direction.South);
        PlaceGreenConveyor(11, 6, Direction.West);
        PlaceGreenConveyor(11, 5, Direction.West);
        
        PlaceBlueConveyor(5, 1, Direction.East);
        PlaceBlueConveyor(8, 5, Direction.West, [Direction.East]);
        PlaceBlueConveyor(10, 5, Direction.North);
        PlaceBlueConveyor(6, 9, Direction.West);
        PlaceBlueConveyor(5, 3, Direction.North, [Direction.East, Direction.South]);
        
        PlaceGreenConveyor(5, 4, Direction.South);
        PlaceGreenConveyor(6,3, Direction.South);
        PlaceGreenConveyor(5,0, Direction.West);
        PlaceGreenConveyor(5,11, Direction.North);
        PlaceGreenConveyor(5, 8, Direction.West, [Direction.South]);
        PlaceGreenConveyor(4,9, Direction.West);
        PlaceGreenConveyor(11,5, Direction.South);
        
        PlaceSpawnPoint(1,12);
        PlaceSpawnPoint(4,13);
        PlaceSpawnPoint(6,13);
        PlaceSpawnPoint(8,12, [Direction.South]);
        
        spaces[2][12] = SpaceFactory.EmptySpace([Direction.South]);
        spaces[2][13] = SpaceFactory.EmptySpace([Direction.South]);
        spaces[2][14] = SpaceFactory.EmptySpace([Direction.South]);
        
        
        spaces[5][12] = SpaceFactory.EmptySpace([Direction.South]);
        spaces[5][13] = SpaceFactory.EmptySpace([Direction.South]);
        spaces[5][14] = SpaceFactory.EmptySpace([Direction.South]);
        
        
        spaces[8][13] = SpaceFactory.EmptySpace([Direction.South]);
        spaces[8][14] = SpaceFactory.EmptySpace([Direction.South]);
        spaces[7][5] = SpaceFactory.EmptySpace([Direction.South]);
        
        PlaceGreenConveyor(3,5 , Direction.South, [Direction.East]);
        PlaceBlueConveyor(3,6 , Direction.South, [Direction.South]);

        
        var board = new GameBoard
        {
            Name = "Starter Course",
            Spaces = spaces
        };

        return board;
        
    }
    
public static GameBoard GetBoard2()
{
    // Create a 15x12 board initialized with empty spaces
    Space.Space[][] spaces = new Space.Space[BoardHeight][];
    for (int row = 0; row < BoardHeight; row++)
    {
        spaces[row] = new Space.Space[BoardWidth];
        for (int col = 0; col < BoardWidth; col++)
        {
            spaces[row][col] = new EmptySpace(Array.Empty<Direction>());
        }
    }

    // ================================
    // Helper methods
    // ================================
    void PlaceBlueConveyor(int r, int c, Direction dir, Direction[]? walls = null) =>
        spaces[r][c] = BoardElementFactory.BlueConveyorBelt(dir, walls);

    void PlaceGreenConveyor(int r, int c, Direction dir, Direction[]? walls = null) =>
        spaces[r][c] = BoardElementFactory.GreenConveyorBelt(dir, walls);

    void PlaceGear(int r, int c, GearDirection rotation, Direction[]? walls = null) =>
        spaces[r][c] = BoardElementFactory.Gear(rotation, walls);

    void PlaceSpawnPoint(int r, int c, Direction[]? walls = null) =>
        spaces[r][c] = SpaceFactory.SpawnPoint(walls);

    void PlaceEmpty(int r, int c, Direction[]? walls = null) =>
        spaces[r][c] = SpaceFactory.EmptySpace(walls);

    // ================================
    // Docking Bay (cols 0–2)
    // ================================
    for (int row = 0; row < BoardHeight; row++)
    {
        PlaceSpawnPoint(row, 2);
    }

    // ================================
    // Outer Green Conveyor Loop
    // (cols 4–13, rows 1–10)
    // ================================
    for (int c = 4; c <= 13; c++)
        PlaceGreenConveyor(1, c, Direction.West);
    for (int r = 2; r <= 10; r++)
        PlaceGreenConveyor(r, 13, Direction.North);
    for (int c = 12; c >= 4; c--)
        PlaceGreenConveyor(10, c, Direction.East);
    for (int r = 9; r >= 2; r--)
        PlaceGreenConveyor(r, 4, Direction.South);

    // ================================
    // Inner Blue Conveyor Loop
    // (cols 6–11, rows 3–8)
    // ================================
    for (int c = 6; c <= 11; c++)
        PlaceBlueConveyor(3, c, Direction.East);
    for (int r = 4; r <= 8; r++)
    {
        // Add wall to position (4, 11)
        if (r == 4)
            PlaceBlueConveyor(r, 11, Direction.South, [Direction.North]);
        else
            PlaceBlueConveyor(r, 11, Direction.South);
    }
    for (int c = 10; c >= 6; c--)
        PlaceBlueConveyor(8, c, Direction.West);
    for (int r = 7; r >= 4; r--)
        PlaceBlueConveyor(r, 6, Direction.North);

    // ================================
    // Inner Gears (like Milk Run)
    // ================================
    PlaceGear(4, 7, GearDirection.ClockWise);
    PlaceGear(4, 10, GearDirection.AntiClockWise);
    PlaceGear(7, 7, GearDirection.AntiClockWise);
    PlaceGear(7, 10, GearDirection.ClockWise);

    // ================================
    // Extra Green Conveyors (Top + Bottom edges)
    // ================================
    PlaceGreenConveyor(0, 8, Direction.South);
    PlaceGreenConveyor(0, 9, Direction.North);
    PlaceGreenConveyor(11, 8, Direction.South);
    PlaceGreenConveyor(11, 9, Direction.North);
    PlaceGreenConveyor(11, 4, Direction.North);

    // ================================
    // Walls - Add to specific positions
    // These need to be placed AFTER the loops or integrated into them
    // ================================
    PlaceEmpty(2, 8, [Direction.West]);
    PlaceEmpty(6, 3, [Direction.North]);
    PlaceEmpty(5, 5, [Direction.North]);
    
    // This one needs special handling - it's in the outer loop
    PlaceGreenConveyor(5, 12, Direction.East, [Direction.North]);
    
    PlaceEmpty(9, 8, [Direction.West]);
    
    // Already handled above in the blue conveyor loop
    // PlaceEmpty(4, 11, [Direction.North]);

    // ================================
    // Finalize and return board
    // ================================
    var board = new GameBoard
    {
        Name = "Castle Tour",
        Spaces = spaces
    };

    return board;
}

}
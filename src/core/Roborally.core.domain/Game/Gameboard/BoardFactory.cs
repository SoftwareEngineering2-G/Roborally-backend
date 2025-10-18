using Roborally.core.domain.Game.Gameboard.BoardElement;
using Roborally.core.domain.Game.Player;

namespace Roborally.core.domain.Game.Gameboard;

using Space;

public static class BoardFactory {
    public const string EmptyBoardName = "Empty Board";
    public const string BoardWithWallsName = "Board With Walls";
    public const string BoardWithWallsAndElementsName = "Board With Walls And Elements";
    
    private static readonly Lock Lock = new ();
    private static readonly Random Random = new();
    private static GameBoard? _emptyBoard;
    private static GameBoard? _boardWithWalls;
    private static GameBoard? _boardWithWallsAndElements;

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
                Name = EmptyBoardName,
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
            
            const int boardSize = 10;
            Space.Space[][] spaces = new Space.Space[boardSize][];
    
            for (int i = 0; i < boardSize; i++) {
                spaces[i] = new Space.Space[boardSize];
                for (int j = 0; j < boardSize; j++)
                {
                    List<Direction> walls = GenerateRandomWalls(
                        j > 0 && spaces[i][j - 1].Walls().Contains(Direction.East),
                        i > 0 && spaces[i - 1][j].Walls().Contains(Direction.South),
                        j == boardSize - 1,
                        i == boardSize - 1
                    );
    
                    spaces[i][j] = new EmptySpace(walls.ToArray());
                }
            }
    
            _boardWithWalls = new GameBoard() {
                Name = BoardWithWallsName,
                Spaces = spaces
            };
            
        }
        
        return _boardWithWalls;
    }

    public static GameBoard GetBoardWithWallsAndElements()
    {
        if (_boardWithWallsAndElements is not null) {
            return _boardWithWallsAndElements;
        }
        
        lock (Lock) {
            // Double-check locking pattern to ensure thread safety
            if (_boardWithWallsAndElements is not null) {
                return _boardWithWallsAndElements;
            }
            
            const int boardSize = 10;
            Space.Space[][] spaces = new Space.Space[boardSize][];
    
            for (int i = 0; i < boardSize; i++) {
                spaces[i] = new Space.Space[boardSize];
                for (int j = 0; j < boardSize; j++)
                {
                    List<Direction> walls = GenerateRandomWalls(
                        j > 0 && spaces[i][j - 1].Walls().Contains(Direction.East),
                        i > 0 && spaces[i - 1][j].Walls().Contains(Direction.South),
                        j == boardSize - 1,
                        i == boardSize - 1
                    );
                    
                    BoardElement.BoardElement? element = GetRandomBoardElement(walls.ToArray());
                    spaces[i][j] = element is not null ? element : new EmptySpace(walls.ToArray());
                }
            }
    
            _boardWithWallsAndElements = new GameBoard() {
                Name = BoardWithWallsAndElementsName,
                Spaces = spaces
            };
            
        }
        
        return _boardWithWallsAndElements;
    }
    
    private static List<Direction> GenerateRandomWalls( bool hasWest, bool hasNorth, bool isOnEasternBorder, bool isOnSouthernBorder)
    {
        List<Direction> walls = new List<Direction>();
        
        if (hasWest) walls.Add(Direction.West);
        if (hasNorth) walls.Add(Direction.North);
        
        // Decide how many additional walls to create (0, 1, 2, or 3)
        // Determine number of walls based on weighted probability
        int rand = Random.Next(100);
        int totalWalls = rand < 50 ? 0 : rand < 75 ? 1 : rand < 95 ? 2 : 3;
        int additionalWalls = totalWalls - walls.Count;
                    
        // Get available directions (not already used and not on outer borders)
        List<Direction> availableDirections = new List<Direction> 
            { Direction.South, Direction.East };
                    
        // Remove directions that would place walls outside the board
        if (isOnSouthernBorder) availableDirections.Remove(Direction.South);
        if (isOnEasternBorder) availableDirections.Remove(Direction.East);
    
        // Add random walls
        for (int w = 0; w < additionalWalls && availableDirections.Count > 0; w++) {
            int index = Random.Next(availableDirections.Count);
            walls.Add(availableDirections[index]);
            availableDirections.RemoveAt(index);
        }
        
        return walls;
    }
    
    private static BoardElement.BoardElement? GetRandomBoardElement(Direction[]? walls = null)
    {
        List<Direction> availableDirections = new List<Direction> 
            { Direction.North, Direction.East, Direction.South, Direction.West };
        
        int rand = Random.Next(100);
        if (rand < 50) return null; // 50% chance of no element
        else if (rand < 70)
        {
            // 20% chance of blue conveyor belt
            Direction direction = availableDirections[Random.Next(availableDirections.Count)];
            return BoardElementFactory.BlueConveyorBelt(direction, walls);
        }
        
        else if (rand < 90)
        {
            // 20% chance of green conveyor belt
            Direction direction = availableDirections[Random.Next(availableDirections.Count)];
            return BoardElementFactory.GreenConveyorBelt(direction, walls);
        }
        
        else
        {
            // 10% chance of gear
            GearDirection gearDirection = Random.Next(2) == 0 ? GearDirection.ClockWise : GearDirection.AntiClockWise;
            return BoardElementFactory.Gear(gearDirection, walls);
        }
    }
}
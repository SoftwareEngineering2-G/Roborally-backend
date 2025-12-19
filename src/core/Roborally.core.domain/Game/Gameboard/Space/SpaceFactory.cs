using Roborally.core.domain.Game.Player;

namespace Roborally.core.domain.Game.Gameboard.Space;

public static class SpaceFactory {
    
    internal const string SpawnPointName = "SpawnPoint";
    internal const string EmptySpaceName = "EmptySpace";
    internal const string CheckpointName = "Checkpoint";


/// <author name="Sachin Baral 2025-09-28 13:55:14 +0200 12" />
    public static Space FromName(string name) {
        switch (name) {
            case EmptySpaceName:
                return new EmptySpace();
            case SpawnPointName:
                return new SpawnPoint();
            case CheckpointName:
                return new Checkpoint { CheckpointNumber = 0 }; // Default, should be set properly when creating board
            default:
                throw new ArgumentException($"Unknown space type: {name}");
        }
    }
    
/// <author name="Truong Son NGO 2025-10-13 13:29:29 +0200 25" />
    public static Space FromNameAndWalls(string name, Direction[] walls) {
        switch (name) {
            case EmptySpaceName:
                return new EmptySpace(walls);
            case SpawnPointName:
                return new SpawnPoint(walls);
            case CheckpointName:
                return new Checkpoint(walls) { CheckpointNumber = 0 }; // Default, should be set properly when creating board
            default:
                throw new ArgumentException($"Unknown space type: {name}");
        }
    }
    
/// <author name="Sachin Baral 2025-10-20 21:20:17 +0200 38" />
    public static SpawnPoint SpawnPoint(Direction[]? walls = null) {
        return new SpawnPoint(walls);
    }

/// <author name="Sachin Baral 2025-10-20 21:20:17 +0200 42" />
    public static EmptySpace EmptySpace(Direction[]? walls = null) {
        return new EmptySpace(walls);
    }
    
/// <author name="Suhani Pandey 2025-11-13 16:20:36 +0100 46" />
    public static Checkpoint Checkpoint(int checkpointNumber, Direction[]? walls = null) {
        return new Checkpoint(walls) { CheckpointNumber = checkpointNumber };
    }
}
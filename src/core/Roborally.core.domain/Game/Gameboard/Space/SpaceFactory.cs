using Roborally.core.domain.Game.Player;

namespace Roborally.core.domain.Game.Gameboard.Space;

public static class SpaceFactory {
    
    internal const string SpawnPointName = "SpawnPoint";
    internal const string EmptySpaceName = "EmptySpace";


    public static Space FromName(string name) {
        switch (name) {
            case EmptySpaceName:
                return new EmptySpace();
            case SpawnPointName:
                return new SpawnPoint();
            default:
                throw new ArgumentException($"Unknown space type: {name}");
        }
    }
    
    public static Space FromNameAndWalls(string name, Direction[] walls) {
        switch (name) {
            case EmptySpaceName:
                return new EmptySpace(walls);
            case SpawnPointName:
                return new SpawnPoint(walls);
            default:
                throw new ArgumentException($"Unknown space type: {name}");
        }
    }
    
    public static SpawnPoint SpawnPoint() {
        return new SpawnPoint();
    }

    public static EmptySpace EmptySpace() {
        return new EmptySpace();
    }
}
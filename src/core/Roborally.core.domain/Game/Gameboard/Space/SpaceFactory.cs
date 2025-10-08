namespace Roborally.core.domain.Game.Gameboard.Space;

public static class SpaceFactory {

    public static Space FromName(string name) {
        switch (name) {
            case "EmptySpace":
                return new EmptySpace();
            case "SpawnPoint1": return new SpawnPoint(1);
            case "SpawnPoint2": return new SpawnPoint(2);
            case "SpawnPoint3": return new SpawnPoint(3);
            case "SpawnPoint4": return new SpawnPoint(4);
            case "SpawnPoint5": return new SpawnPoint(5);
            case "SpawnPoint6": return new SpawnPoint(6);
            default:
                throw new ArgumentException($"Unknown space type: {name}");
        }
    }
}
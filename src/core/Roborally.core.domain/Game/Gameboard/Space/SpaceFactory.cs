namespace Roborally.core.domain.Game.Gameboard.Space;

public static class SpaceFactory {

    public static Space FromName(string name) {
        switch (name) {
            case "EmptySpace":
                return new EmptySpace();
            case "SpawnPoint":
                return new SpawnPoints();
            default:
                throw new ArgumentException($"Unknown space type: {name}");
        }
    }
}
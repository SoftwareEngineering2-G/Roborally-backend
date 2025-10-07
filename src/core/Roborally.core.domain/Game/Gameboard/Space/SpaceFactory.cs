namespace Roborally.core.domain.Game.Gameboard.Space;

public static class SpaceFactory {

    public static Space FromName(string name) {
        switch (name) {
            case "EmptySpace":
                return new EmptySpace();
            case "SpawnPoint":
                return new SpawnPoints();
            case "Checkpoint1":
                return new Checkpoint(1);
            case "Checkpoint2":
                return new Checkpoint(2);
            case "Checkpoint3":
                return new Checkpoint(3);
            case "Checkpoint4":
                return new Checkpoint(4);
            case "Checkpoint5":
                return new Checkpoint(5);
            case "Checkpoint6":
                return new Checkpoint(6);
            default:
                throw new ArgumentException($"Unknown space type: {name}");
        }
    }
}
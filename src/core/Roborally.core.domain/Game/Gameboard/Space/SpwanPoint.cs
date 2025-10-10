using Roborally.core.domain.Game.Player;

namespace Roborally.core.domain.Game.Gameboard.Space;

public class SpawnPoints : Space {
    public SpawnPoints(Direction[]? walls = null) : base(walls) {}
    
    public override string Name() {
        return "SpawnPoint";
    }
}
using Roborally.core.domain.Game.Player;

namespace Roborally.core.domain.Game.Gameboard.Space;

public class SpawnPoint : Space {
    public SpawnPoint(Direction[]? walls = null) : base(walls) {}
    
    public override string Name() {
        return SpaceFactory.SpawnPointName;
    }
}
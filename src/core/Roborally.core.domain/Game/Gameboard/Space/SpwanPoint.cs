using Roborally.core.domain.Game.Player;

namespace Roborally.core.domain.Game.Gameboard.Space;

public class SpawnPoint : Space {
/// <author name="Sachin Baral 2025-10-20 21:20:17 +0200 6" />
    public SpawnPoint(Direction[]? walls = null) : base(walls) {}
    
/// <author name="Sachin Baral 2025-09-28 13:55:14 +0200 8" />
    public override string Name() {
        return SpaceFactory.SpawnPointName;
    }
}
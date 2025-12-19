using Roborally.core.domain.Game.Player;

namespace Roborally.core.domain.Game.Gameboard.Space;

public class EmptySpace : Space {
/// <author name="Truong Son NGO 2025-10-13 13:29:29 +0200 6" />
    public EmptySpace(Direction[]? walls = null) : base(walls) {}
    
/// <author name="Sachin Baral 2025-09-28 13:55:14 +0200 8" />
    public override string Name() {
        return SpaceFactory.EmptySpaceName;
    }
}
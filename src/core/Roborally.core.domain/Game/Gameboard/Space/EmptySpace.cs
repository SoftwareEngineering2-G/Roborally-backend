using Roborally.core.domain.Game.Player;

namespace Roborally.core.domain.Game.Gameboard.Space;

public class EmptySpace : Space {
    public EmptySpace(Direction[]? walls = null) : base(walls) {}
    
    public override string Name() {
        return SpaceFactory.EmptySpaceName;
    }
}
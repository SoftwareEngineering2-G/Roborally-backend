using Roborally.core.domain.Game.Player;

namespace Roborally.core.domain.Game.Gameboard.BoardElement;

public class Gear : BoardElement {
    public required GearDirection Direction { get; set; }

    public override string Name() {
        return BoardElementFactory.GearName;
    }
    
    public Gear(Direction[]? walls = null) : base(walls) {
    }
}
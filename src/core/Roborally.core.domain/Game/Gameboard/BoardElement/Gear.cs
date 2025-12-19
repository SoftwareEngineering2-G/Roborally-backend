namespace Roborally.core.domain.Game.Gameboard.BoardElement;

public class Gear : BoardElement {
    public GearDirection Direction { get; set; }

/// <author name="Nilanjana Devkota 2025-10-14 19:37:00 +0200 6" />
    public override string Name() {
        return BoardElementFactory.GearName;
    }
    
/// <author name="Nilanjana Devkota 2025-10-19 11:13:58 +0200 10" />
    internal Gear() {
    }
    
/// <author name="Nilanjana Devkota 2025-10-19 11:13:58 +0200 13" />
    internal Gear(Player.Direction[] walls) : base(walls) {
    }
}
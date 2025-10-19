namespace Roborally.core.domain.Game.Gameboard.BoardElement;

public class Gear : BoardElement {
    public GearDirection Direction { get; set; }

    public override string Name() {
        return BoardElementFactory.GearName;
    }
    
    internal Gear() {
    }
    
    internal Gear(Player.Direction[] walls) : base(walls) {
    }
}
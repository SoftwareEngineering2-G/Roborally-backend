namespace Roborally.core.domain.Game.Gameboard.BoardElement;

public class PriorityAntenna : BoardElement {
    public override string Name() {
        return BoardElementFactory.PriorityAntennaName;
    }
    
    internal PriorityAntenna() {
    }
    
    internal PriorityAntenna(Player.Direction[] walls) : base(walls) {
    }
}

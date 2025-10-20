using Roborally.core.domain.Game.Player;

namespace Roborally.core.domain.Game.Gameboard.BoardElement;

public class BlueConveyorBelt : BoardElement {
    public required Direction Direction { get; set; }


    public override string Name() {
        return BoardElementFactory.BlueConveyorBeltName;
    }


    internal BlueConveyorBelt() {
    }
    
    internal BlueConveyorBelt(Direction[]? walls) : base(walls) {
    }
}

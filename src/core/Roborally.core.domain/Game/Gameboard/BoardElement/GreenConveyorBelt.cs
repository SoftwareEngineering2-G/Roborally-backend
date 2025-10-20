using Roborally.core.domain.Game.Player;

namespace Roborally.core.domain.Game.Gameboard.BoardElement;

public class GreenConveyorBelt : BoardElement {
    public required Direction Direction { get; set; }

    public override string Name() {
        return BoardElementFactory.GreenConveyorBeltName;
    }


    internal GreenConveyorBelt() {
    }
    
    internal GreenConveyorBelt(Direction[]? walls) : base(walls) {
    }
}
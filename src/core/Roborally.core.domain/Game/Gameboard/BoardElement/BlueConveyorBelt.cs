using Roborally.core.domain.Game.Player;

namespace Roborally.core.domain.Game.Gameboard.BoardElement;

public class BlueConveyorBelt : BoardElement {
    public required Direction Direction { get; set; }


/// <author name="Nilanjana Devkota 2025-10-14 19:37:00 +0200 9" />
    public override string Name() {
        return BoardElementFactory.BlueConveyorBeltName;
    }


/// <author name="Nilanjana Devkota 2025-10-14 19:37:00 +0200 14" />
    internal BlueConveyorBelt() {
    }
    
/// <author name="Sachin Baral 2025-10-20 21:20:17 +0200 17" />
    internal BlueConveyorBelt(Direction[]? walls) : base(walls) {
    }
}
using Roborally.core.domain.Game.Player;

namespace Roborally.core.domain.Game.Gameboard.BoardElement;

public class GreenConveyorBelt : BoardElement {
    public required Direction Direction { get; set; }

/// <author name="nilanjanadevkota 2025-10-14 19:37:00 +0200 8" />
    public override string Name() {
        return BoardElementFactory.GreenConveyorBeltName;
    }


/// <author name="nilanjanadevkota 2025-10-14 19:37:00 +0200 13" />
    internal GreenConveyorBelt() {
    }
    
/// <author name="Sachin Baral 2025-10-20 21:20:17 +0200 16" />
    internal GreenConveyorBelt(Direction[]? walls) : base(walls) {
    }
}
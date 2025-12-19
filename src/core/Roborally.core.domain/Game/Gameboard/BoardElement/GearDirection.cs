using Roborally.core.domain.Bases;

namespace Roborally.core.domain.Game.Gameboard.BoardElement;

public class GearDirection : Enumeration {

/// <author name="nilanjanadevkota 2025-10-14 19:37:00 +0200 7" />
    public static readonly GearDirection ClockWise = new("ClockWise");
/// <author name="nilanjanadevkota 2025-10-14 19:37:00 +0200 8" />
    public static readonly GearDirection AntiClockWise = new("AntiClockWise");



/// <author name="nilanjanadevkota 2025-10-14 19:37:00 +0200 12" />
    private GearDirection(string value) : base(value){}
    
}
using Roborally.core.domain.Bases;

namespace Roborally.core.domain.Game.Gameboard.BoardElement;

public class GearDirection : Enumeration {

    public static readonly GearDirection ClockWise = new("ClockWise");
    public static readonly GearDirection AntiClockWise = new("AntiClockWise");

    public static GearDirection FromDisplayName(string displayName) {
        return displayName switch {
            "ClockWise" => ClockWise,
            "AntiClockWise" => AntiClockWise,
            _ => throw new ArgumentException($"Invalid GearDirection display name: {displayName}")
        };
    }

    private GearDirection(string value) : base(value){}
    
}
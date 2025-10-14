using Roborally.core.domain.Bases;

namespace Roborally.core.domain.Game.Gameboard.BoardElement;

public class GearDirection : Enumeration {

    public static readonly GearDirection ClockWise = new("ClockWise");
    public static readonly GearDirection AntiClockWise = new("AntiClockWise");



    private GearDirection(string value) : base(value){}
    
}
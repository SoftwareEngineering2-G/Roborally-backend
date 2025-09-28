using Roborally.core.domain.Bases;

namespace Roborally.core.domain.Game.Player;

public class Robot : Enumeration{


    public static readonly Robot Red = new Robot("Red");
    public static readonly Robot Blue = new Robot("Blue");
    public static readonly Robot Green = new Robot("Green");
    public static readonly Robot Yellow = new Robot("Yellow");
    public static readonly Robot Black = new Robot("Black");
    public static readonly Robot White = new Robot("White");

    private Robot(string displayName) : base(displayName) {
        
    }

    public static Robot[] All() {
        return [Red, Blue, Green, Yellow, Black , White];
    }
    
}
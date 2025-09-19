using Roborally.core.domain.Bases;

namespace Roborally.core.domain.Game;

public class Robot : Enumeration{


    public static readonly Robot RED = new Robot("Red");
    public static readonly Robot BLUE = new Robot("Blue");
    public static readonly Robot GREEN = new Robot("Green");
    public static readonly Robot YELLOW = new Robot("Yellow");
    public static readonly Robot BLACK = new Robot("Black");
    public static readonly Robot WHITE = new Robot("White");

    private Robot(string displayName) : base(displayName) {
        
    }
    
}
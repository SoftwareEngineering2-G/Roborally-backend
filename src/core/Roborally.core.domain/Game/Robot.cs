using Roborally.core.domain.Bases;

namespace Roborally.core.domain.Game;

public class Robot : Enumeration{


    private readonly Robot RED = new Robot("Red");
    private readonly Robot BLUE = new Robot("Blue");
    private readonly Robot GREEN = new Robot("Green");
    private readonly Robot YELLOW = new Robot("Yellow");
    private readonly Robot BLACK = new Robot("Black");
    private readonly Robot WHITE = new Robot("White");

    private Robot(string displayName) : base(displayName) {
        
    }
    
}
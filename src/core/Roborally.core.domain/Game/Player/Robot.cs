using Roborally.core.domain.Bases;

namespace Roborally.core.domain.Game.Player;

public class Robot : Enumeration{


/// <author name="Sachin Baral 2025-09-28 13:55:14 +0200 8" />
    public static readonly Robot Red = new Robot("Red");
/// <author name="Sachin Baral 2025-09-28 13:55:14 +0200 9" />
    public static readonly Robot Blue = new Robot("Blue");
/// <author name="Sachin Baral 2025-09-28 13:55:14 +0200 10" />
    public static readonly Robot Green = new Robot("Green");
/// <author name="Sachin Baral 2025-09-28 13:55:14 +0200 11" />
    public static readonly Robot Yellow = new Robot("Yellow");
/// <author name="Sachin Baral 2025-09-28 13:55:14 +0200 12" />
    public static readonly Robot Black = new Robot("Black");
/// <author name="Sachin Baral 2025-09-28 13:55:14 +0200 13" />
    public static readonly Robot White = new Robot("White");

/// <author name="Sachin Baral 2025-09-28 13:55:14 +0200 15" />
    private Robot(string displayName) : base(displayName) {
        
    }

/// <author name="Sachin Baral 2025-09-28 13:55:14 +0200 19" />
    public static Robot[] All() {
        return [Red, Blue, Green, Yellow, Black , White];
    }
    
}
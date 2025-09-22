using Roborally.core.domain.Bases;

namespace Roborally.core.domain.Game;

public class Direction : Enumeration
{
    public static readonly Direction North = new Direction("North");
    public static readonly Direction South = new Direction("South");
    public static readonly Direction East = new Direction("East");
    public static readonly Direction West = new Direction("West");

    private Direction(string displayName) : base(displayName)
    {
    }
    
    public static Direction From(string name) => name switch
    {
        "North" => North,
        "South" => South,
        "East"  => East,
        "West"  => West,
        _ => throw new ArgumentOutOfRangeException(nameof(name), name, "Unknown Direction")
    };
}
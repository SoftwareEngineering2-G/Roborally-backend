using Roborally.core.domain.Bases;

namespace Roborally.core.domain.Game.Player;

public class Direction : Enumeration
{
    public static readonly Direction North = new Direction("North");
    public static readonly Direction South = new Direction("South");
    public static readonly Direction East = new Direction("East");
    public static readonly Direction West = new Direction("West");

    private Direction(string displayName) : base(displayName)
    {
    }
}
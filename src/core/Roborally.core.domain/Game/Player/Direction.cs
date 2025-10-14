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
    
    public Position GetNextPosition(Position currentPosition)
    {
        return DisplayName switch
        {
            "North" => new Position(currentPosition.X, currentPosition.Y - 1),
            "South" => new Position(currentPosition.X, currentPosition.Y + 1),
            "East" => new Position(currentPosition.X + 1, currentPosition.Y),
            "West" => new Position(currentPosition.X - 1, currentPosition.Y),
            _ => throw new CustomException($"Invalid direction: {DisplayName}", 500)
        };
    }

    public Position GetPositionBehind(Position currentPosition)
    {
        return DisplayName switch
        {
            "North" => new Position(currentPosition.X, currentPosition.Y + 1),
            "South" => new Position(currentPosition.X, currentPosition.Y - 1),
            "East" => new Position(currentPosition.X - 1, currentPosition.Y),
            "West" => new Position(currentPosition.X + 1, currentPosition.Y),
            _ => throw new CustomException($"Invalid direction: {DisplayName}", 500)
        };
    }

    public Direction RotateLeft()
    {
        return DisplayName switch
        {
            "North" => West,
            "West" => South,
            "South" => East,
            "East" => North,
            _ => throw new CustomException($"Invalid direction: {DisplayName}", 500)
        };
    }
    
    public Direction RotateRight()
    {
        return DisplayName switch
        {
            "North" => East,
            "East" => South,
            "South" => West,
            "West" => North,
            _ => throw new CustomException($"Invalid direction: {DisplayName}", 500)
        };
    }
    
    public Direction Opposite()
    {
        return DisplayName switch
        {
            "North" => South,
            "South" => North,
            "East" => West,
            "West" => East,
            _ => throw new CustomException($"Invalid direction: {DisplayName}", 500)
        };
    }

    public static Direction FromDisplayName(string displayName)
    {
        return displayName.ToLower() switch
        {
            "north" => North,
            "south" => South,
            "east" => East,
            "west" => West,
            _ => throw new ArgumentException($"Invalid direction display name: {displayName}")
        };
    }
}
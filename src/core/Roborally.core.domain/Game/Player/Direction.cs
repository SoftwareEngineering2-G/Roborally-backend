using Roborally.core.domain.Bases;

namespace Roborally.core.domain.Game.Player;

public class Direction : Enumeration
{
/// <author name="Sachin Baral 2025-09-16 15:57:05 +0200 7" />
    public static readonly Direction North = new Direction("North");
/// <author name="Sachin Baral 2025-09-16 15:57:05 +0200 8" />
    public static readonly Direction South = new Direction("South");
/// <author name="Sachin Baral 2025-09-16 15:57:05 +0200 9" />
    public static readonly Direction East = new Direction("East");
/// <author name="Sachin Baral 2025-09-16 15:57:05 +0200 10" />
    public static readonly Direction West = new Direction("West");

/// <author name="Truong Son NGO 2025-09-19 17:04:26 +0200 12" />
    private Direction(string displayName) : base(displayName)
    {
    }
    
/// <author name="Suhani Pandey 2025-10-10 13:01:53 +0200 16" />
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

/// <author name="Suhani Pandey 2025-10-10 13:01:53 +0200 28" />
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

/// <author name="Suhani Pandey 2025-10-10 13:01:53 +0200 40" />
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
    
/// <author name="Suhani Pandey 2025-10-10 13:01:53 +0200 52" />
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
    
/// <author name="Suhani Pandey 2025-10-10 13:01:53 +0200 64" />
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

/// <author name="Truong Son NGO 2025-10-13 13:29:29 +0200 76" />
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
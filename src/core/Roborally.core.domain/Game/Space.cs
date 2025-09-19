namespace Roborally.core.domain.Game;

public abstract class Space
{
    public List<Direction> WallDirection { get; init; } = [];


    public bool HasWalls()
    {
        return WallDirection.Any();
    }
}
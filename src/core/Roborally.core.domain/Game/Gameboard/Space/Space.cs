using Roborally.core.domain.Game.Player;

namespace Roborally.core.domain.Game.Gameboard.Space;

public abstract class Space
{
    private readonly Direction[] _walls;
    
/// <author name="Truong Son NGO 2025-10-13 13:29:29 +0200 9" />
    public Direction[] Walls()
    {
        return _walls;
    }
    
/// <author name="Truong Son NGO 2025-10-13 13:29:29 +0200 14" />
    protected Space(Direction[]? walls = null)
    {
        _walls = walls ?? [];
    }
    
/// <author name="Sachin Baral 2025-09-28 13:55:14 +0200 19" />
    public abstract string Name();
}
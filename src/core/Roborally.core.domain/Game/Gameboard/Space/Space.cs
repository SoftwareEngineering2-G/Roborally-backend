using Roborally.core.domain.Game.Player;

namespace Roborally.core.domain.Game.Gameboard.Space;

public abstract class Space
{
    private Direction[] _walls;
    
    public Direction[] Walls()
    {
        return _walls;
    }
    
    protected Space( Direction[]? walls = null)
    {
        _walls = walls ?? Array.Empty<Direction>();
    }
    
    public abstract string Name();
}
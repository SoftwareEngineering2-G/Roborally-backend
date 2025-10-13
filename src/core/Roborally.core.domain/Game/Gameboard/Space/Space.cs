using Roborally.core.domain.Game.Player;

namespace Roborally.core.domain.Game.Gameboard.Space;

public abstract class Space
{
    private readonly Direction[] _walls;
    
    public Direction[] Walls()
    {
        return _walls;
    }
    
    protected Space(Direction[]? walls = null)
    {
        _walls = walls ?? [];
    }
    
    public abstract string Name();
}
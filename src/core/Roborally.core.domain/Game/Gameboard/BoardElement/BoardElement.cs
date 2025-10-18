using Roborally.core.domain.Game.Player;

namespace Roborally.core.domain.Game.Gameboard.BoardElement;

public abstract class BoardElement : Space.Space
{
    protected BoardElement(Direction[]? walls = null) : base(walls)
    {
    }
}                                                  
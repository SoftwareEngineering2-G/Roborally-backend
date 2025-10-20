namespace Roborally.core.domain.Game.Gameboard.BoardElement;

public abstract class BoardElement : Space.Space
{
    protected BoardElement() : base()
    {
    }
    
    protected BoardElement(Player.Direction[]? walls) : base(walls)
    {
    }
}

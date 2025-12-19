namespace Roborally.core.domain.Game.Gameboard.BoardElement;

public abstract class BoardElement : Space.Space
{
/// <author name="Nilanjana Devkota 2025-10-19 11:13:58 +0200 5" />
    protected BoardElement() : base()
    {
    }
    
/// <author name="Sachin Baral 2025-10-20 21:20:17 +0200 9" />
    protected BoardElement(Player.Direction[]? walls) : base(walls)
    {
    }
}
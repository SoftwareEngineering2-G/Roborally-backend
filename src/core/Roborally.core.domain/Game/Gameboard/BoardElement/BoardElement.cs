namespace Roborally.core.domain.Game.Gameboard.BoardElement;

public abstract class BoardElement : Space.Space
{
    public abstract void Activate(Player.Player player);
}
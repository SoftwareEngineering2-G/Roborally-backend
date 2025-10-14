namespace Roborally.core.domain.Game.Gameboard.BoardElement.BoardElementActivationStrategies;

public interface IBoardElementActivationStrategy {
    void Activate(Game game, Player.Player player, BoardElement boardElement);
}
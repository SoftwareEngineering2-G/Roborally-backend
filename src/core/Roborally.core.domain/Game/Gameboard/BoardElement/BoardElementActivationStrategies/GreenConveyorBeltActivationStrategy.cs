namespace Roborally.core.domain.Game.Gameboard.BoardElement.BoardElementActivationStrategies;

public class GreenConveyorBeltActivationStrategy : IBoardElementActivationStrategy{


    public void Activate(Game game, Player.Player player, BoardElement boardElement) {

        if (boardElement is not GreenConveyorBelt greenConveyorBelt) {
            throw new ArgumentException("boardElement must be of type GreenConveyorBelt");
        }

        game.MovePlayerInDirection(player, greenConveyorBelt.Direction, shouldPush: false);
    }
}
namespace Roborally.core.domain.Game.Gameboard.BoardElement.BoardElementActivationStrategies;

public class GreenConveyorBeltActivationStrategy : IBoardElementActivationStrategy{


/// <author name="nilanjanadevkota 2025-10-14 19:37:00 +0200 6" />
    public void Activate(Game game, Player.Player player, BoardElement boardElement) {

        if (boardElement is not GreenConveyorBelt greenConveyorBelt) {
            throw new ArgumentException("boardElement must be of type GreenConveyorBelt");
        }

        game.MovePlayerInDirection(player, greenConveyorBelt.Direction, shouldPush: false);  // Green conveyor belt does not push other players
    }
}
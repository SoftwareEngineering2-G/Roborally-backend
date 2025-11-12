namespace Roborally.core.domain.Game.Gameboard.BoardElement.BoardElementActivationStrategies;

public class BlueConveyorBeltActivationStrategy : IBoardElementActivationStrategy {
    public void Activate(Game game, Player.Player player, BoardElement boardElement) {
        if (boardElement is not BlueConveyorBelt blueConveyorBelt) {
            throw new ArgumentException("boardElement must be of type BlueConveyorBelt");
        }

        // Note: We don't pass systemTime here because checkpoint checking happens in Game.ActivateNextBoardElement
        game.MovePlayerInDirection(player, blueConveyorBelt.Direction, shouldPush: false);

        Space.Space space = game.GameBoard.GetSpaceAt(player.CurrentPosition);      

        // Only push again, if the player is still on a blue conveyor belt
        if (space is BlueConveyorBelt) {
            game.MovePlayerInDirection(player, blueConveyorBelt.Direction, shouldPush: false);
        }

    }
}
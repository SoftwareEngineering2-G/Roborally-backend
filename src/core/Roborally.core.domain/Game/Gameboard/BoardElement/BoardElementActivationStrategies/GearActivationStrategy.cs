namespace Roborally.core.domain.Game.Gameboard.BoardElement.BoardElementActivationStrategies;

public class GearActivationStrategy : IBoardElementActivationStrategy {
    public void Activate(Game game, Player.Player player, BoardElement boardElement) {
        if (boardElement is not Gear gear) {
            throw new ArgumentException("boardElement must be of type Gear");
        }

        if (gear.Direction.Equals(GearDirection.ClockWise)) {
            player.RotateRight();
        }
        else if (gear.Direction.Equals(GearDirection.AntiClockWise)) {
            player.RotateLeft();
        }
    }
}
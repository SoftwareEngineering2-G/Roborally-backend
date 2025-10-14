using Roborally.core.domain.Game.Player;

namespace Roborally.core.domain.Game;

// This class is just a game class that contains all movement logic.
// I made this class so that the game class does not become too bloated.   - Nilanjana
public static class GameMovementExtension {
    // bool returns if the move was successful
    public static bool MovePlayerInDirection(this Game game, Player.Player player, Direction direction,
        bool shouldPush = true) {
        Position nextPosition = player.GetNextPosition(direction);

        if (!game.GameBoard.IsWithinBounds(nextPosition)) {
            return false;
            // throw new CustomException($"Cannot move {player.Username}: Robot would fall off the board.", 400);
        }

        if (game.GameBoard.HasWallBetween(player.CurrentPosition, nextPosition, player.CurrentFacingDirection)) {
            // We do not move
            return false;
        }

        // If there exists already a player
        var existingPlayer = game.Players.FirstOrDefault(p =>
            !p.Username.Equals(player.Username) && p.CurrentPosition.Equals(nextPosition));

        // If someone exists, push them
        if (existingPlayer is not null) {
            if (!shouldPush) {
                return false;      // If push is disabled, we dont push the other robot and cannot move either...                  
            }

            // Recursion to push the existing player
            bool wasPushed = game.MovePlayerInDirection(existingPlayer, direction);
            if (!wasPushed) return false;
        }

        // If there is no player blocking, simply make a move
        player.MoveTo(nextPosition);
        return true;
    }
}
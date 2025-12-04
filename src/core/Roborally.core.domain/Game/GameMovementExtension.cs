using Roborally.core.domain.Bases;
using Roborally.core.domain.Game.Gameboard.Space;
using Roborally.core.domain.Game.GameEvents;
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
                return
                    false; // If push is disabled, we dont push the other robot and cannot move either...
            }

            // Check if the robot can be pushed (only one robot at a time)
            Position positionBehindExistingPlayer = existingPlayer.GetNextPosition(direction);
            
            // Check if the position behind is out of bounds
            if (!game.GameBoard.IsWithinBounds(positionBehindExistingPlayer)) {
                return false; // Cannot push robot off the board
            }
            
            // Check if there's a wall blocking the push
            if (game.GameBoard.HasWallBetween(existingPlayer.CurrentPosition, positionBehindExistingPlayer, existingPlayer.CurrentFacingDirection)) {
                return false; // Wall blocks the push
            }
            
            // Check if there's ANOTHER robot behind the one we're trying to push
            var secondRobot = game.Players.FirstOrDefault(p =>
                !p.Username.Equals(player.Username) && 
                !p.Username.Equals(existingPlayer.Username) && 
                p.CurrentPosition.Equals(positionBehindExistingPlayer));
            
            if (secondRobot is not null) {
                return false; // Cannot push two or more robots at once
            }

            // Only one robot to push, so push them
            existingPlayer.MoveTo(positionBehindExistingPlayer);
        }

        // If there is no player blocking, simply make a move
        player.MoveTo(nextPosition);
        return true;
    }

    /// <summary>
    /// Checks if the player is on a checkpoint
    /// </summary>
    public static async Task CheckAndRecordCheckpoint(this Game game, ISystemTime systemTime) {

        // Check if the player is on a checkpoint
        foreach (var player in game.Players) {
            Space space = game.GameBoard.GetSpaceAt(player.CurrentPosition);

            if (space is not Checkpoint checkpoint) {
                continue; // Not on a checkpoint
            }

            // Get total number of checkpoints on the board
            int totalCheckpoints = game.GameBoard.GetAllSpacesOfType<Checkpoint>().Count;

            // Use the Player's ReachCheckpoint method which handles the sequential logic
            player.ReachCheckpoint(checkpoint, systemTime);

            if (player.HasCompletedAllCheckpoints(totalCheckpoints)) {
              await game.HandleGameCompleted(player, systemTime);
            }

        }
    }
}
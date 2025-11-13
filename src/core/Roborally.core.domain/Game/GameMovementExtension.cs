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
    
    /// <summary>
    /// Checks if the player is on a checkpoint and records the event if they reached the next checkpoint in sequence.
    /// </summary>
    public static void CheckAndRecordCheckpoint(this Game game, Player.Player player, ISystemTime systemTime) {
        Space space = game.GameBoard.GetSpaceAt(player.CurrentPosition);
        
        if (space is not Checkpoint checkpoint) {
            return; // Not on a checkpoint
        }
        
        // Get total number of checkpoints on the board
        int totalCheckpoints = game.GameBoard.GetAllSpacesOfType<Checkpoint>().Count;
        
        // Use the Player's ReachCheckpoint method which handles the sequential logic
        player.ReachCheckpoint(checkpoint, totalCheckpoints);
        
        // Only record the event if the checkpoint was actually reached (player passed the check)
        if (player.CurrentCheckpointPassed == checkpoint.CheckpointNumber) {
            // This means the player just reached this checkpoint
            CheckpointReachedEvent checkpointEvent = new CheckpointReachedEvent {
                GameId = game.GameId,
                HappenedAt = systemTime.CurrentTime,
                Username = player.Username,
                CheckpointNumber = checkpoint.CheckpointNumber
            };
            
            game.GameEvents.Add(checkpointEvent);
        }
    }
}
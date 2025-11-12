using Roborally.core.domain.Game.Player;
using Roborally.core.domain.Game.Gameboard.Space;

namespace Roborally.core.domain.Game;

// This class is just a game class that contains all movement logic.
// I made this class so that the game class does not become too bloated.   - Nilanjana
public static class GameMovementExtension {
    // bool returns if the move was successful
    public static bool MovePlayerInDirection(this Game game, Player.Player player, Direction direction,
        bool shouldPush = true, Bases.ISystemTime? systemTime = null) {
        Position nextPosition = player.GetNextPosition(direction);

        if (!game.GameBoard.IsWithinBounds(nextPosition)) {
            return false;
        }

        if (game.GameBoard.HasWallBetween(player.CurrentPosition, nextPosition, player.CurrentFacingDirection)) {
            return false;
        }

        var existingPlayer = game.Players.FirstOrDefault(p =>
            !p.Username.Equals(player.Username) && p.CurrentPosition.Equals(nextPosition));

        if (existingPlayer is not null) {
            if (!shouldPush) {
                return false;
            }

            // Recursively push the existing player - they also get checkpoint checked!
            bool wasPushed = game.MovePlayerInDirection(existingPlayer, direction, shouldPush, systemTime);
            if (!wasPushed) return false;
            // Note: The pushed player's checkpoint is already checked in the recursive call above
        }

        player.MoveTo(nextPosition);
        
        // Check checkpoint immediately after EACH move step if systemTime is provided
        if (systemTime != null)
        {
            game.CheckPlayerOnCheckpoint(player, systemTime);
        }
        
        return true;
    }
    
    // Check if player landed on a checkpoint and create event if so
    public static void CheckPlayerOnCheckpoint(this Game game, Player.Player player, Bases.ISystemTime systemTime)
    {
        var space = game.GameBoard.GetSpaceAt(player.CurrentPosition);
        
        if (space is not CheckpointSpace checkpoint) 
            return;

        int previousProgress = player.CurrentCheckpointPassed;
        bool hasWon = player.ReachCheckpoint(checkpoint, game.GameBoard.TotalCheckpoints);
        
        bool checkpointReached = player.CurrentCheckpointPassed > previousProgress;

        if (checkpointReached)
        {
            Console.WriteLine($"[CHECKPOINT] Player {player.Username} reached checkpoint {player.CurrentCheckpointPassed}!");
            
            // Create and add checkpoint reached event
            var checkpointEvent = new GameEvents.CheckpointReachedEvent
            {
                GameId = game.GameId,
                Username = player.Username,
                CheckpointNumber = player.CurrentCheckpointPassed,
                HappenedAt = systemTime.CurrentTime
            };
            game.GameEvents.Add(checkpointEvent);
        }

        if (hasWon)
        {
            game.SetWinner(player);
        }
    }
}
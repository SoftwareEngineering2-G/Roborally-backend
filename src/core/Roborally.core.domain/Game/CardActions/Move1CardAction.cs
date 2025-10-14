using Roborally.core.domain.Game.Player;

namespace Roborally.core.domain.Game.CardActions;

public class Move1CardAction : ICardAction {
    public void Execute(Player.Player player, Game game, List<Player.Player> players) {
        // We dont need them, since the params is not with the ? sign
        // if (player == null) throw new CustomException("Player cannot be null.", 400);
        // if (game == null) throw new CustomException("Game cannot be null.", 400);

        game.MovePlayerInDirection(player, player.CurrentFacingDirection);

        // Save this action as the last executed action for this player
        player.LastExecutedAction = this;
    }

    // private void MovePlayerForward(Player.Player player, Game game, List<Player.Player> allPlayers, int spaces)
    // {
    //     for (int i = 0; i < spaces; i++)
    //     {
    //         Position nextPosition = player.GetNextPosition();
    //
    //         // Check if next position is within board bounds
    //         if (!game.GameBoard.IsWithinBounds(nextPosition))
    //         {
    //             throw new CustomException($"Cannot move {player.Username}: Robot would fall off the board.", 400);
    //         }
    //
    //         // Check for walls blocking the movement
    //         if (game.GameBoard.HasWallBetween(player.CurrentPosition, nextPosition, player.CurrentFacingDirection))
    //         {
    //             throw new CustomException($"Cannot move {player.Username}: Wall is blocking the path.", 400);
    //         }
    //
    //         // Check if the space is occupied by another player
    //         var occupyingPlayer = allPlayers.FirstOrDefault(p => p.CurrentPosition == nextPosition && p.Username != player.Username);
    //
    //         if (occupyingPlayer != null)
    //         {
    //             // Try to push the occupying player in the same direction
    //             PushPlayer(occupyingPlayer, player.CurrentFacingDirection, game, allPlayers);
    //         }
    //
    //         // Move the player to the next position
    //         player.MoveTo(nextPosition);
    //     }
    // }

    // private void PushPlayer(Player.Player playerToPush, Direction pushDirection, Game game, List<Player.Player> allPlayers)
    // {
    //     Position pushedPosition = GetNextPositionInDirection(playerToPush.CurrentPosition, pushDirection);
    //
    //     // Check if pushed position is within bounds
    //     if (!game.GameBoard.IsWithinBounds(pushedPosition))
    //     {
    //         throw new CustomException($"Cannot push {playerToPush.Username}: Robot would be pushed off the board.", 400);
    //     }
    //
    //     // Check for walls blocking the push
    //     if (game.GameBoard.HasWallBetween(playerToPush.CurrentPosition, pushedPosition, pushDirection))
    //     {
    //         throw new CustomException($"Cannot push {playerToPush.Username}: Wall is blocking the push.", 400);
    //     }
    //
    //     // Check if there's another player in the pushed position (chain pushing)
    //     var nextOccupyingPlayer = allPlayers.FirstOrDefault(p => p.CurrentPosition == pushedPosition && p.Username != playerToPush.Username);
    //
    //     if (nextOccupyingPlayer != null)
    //     {
    //         // Recursively push the next player
    //         PushPlayer(nextOccupyingPlayer, pushDirection, game, allPlayers);
    //     }
    //
    //     // Push the player to the new position
    //     playerToPush.MoveTo(pushedPosition);
    // }
    //
    // private Position GetNextPositionInDirection(Position position, Direction direction)
    // {
    //     return direction.DisplayName switch
    //     {
    //         "North" => new Position(position.X, position.Y - 1),
    //         "South" => new Position(position.X, position.Y + 1),
    //         "East" => new Position(position.X + 1, position.Y),
    //         "West" => new Position(position.X - 1, position.Y),
    //         _ => throw new CustomException($"Invalid direction: {direction.DisplayName}", 500)
    //     };
    // }
}
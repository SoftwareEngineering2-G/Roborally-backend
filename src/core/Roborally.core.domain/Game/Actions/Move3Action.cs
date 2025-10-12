using Roborally.core.domain.Bases;
using Roborally.core.domain.Game.Player;

namespace Roborally.core.domain.Game.Actions;

public class Move3Action : IAction
{
    public void Execute(Player.Player player, Game game, List<Player.Player> players)
    {
        if (player == null) throw new CustomException("Player cannot be null.", 400);
        if (game == null) throw new CustomException("Game cannot be null.", 400);

        MovePlayerForward(player, game, players, 3);
        
        // Save this action as the last executed action for this player
        player.LastExecutedAction = this;
    }

    private void MovePlayerForward(Player.Player player, Game game, List<Player.Player> allPlayers, int spaces)
    {
        for (int i = 0; i < spaces; i++)
        {
            Position nextPosition = player.GetNextPosition(player.CurrentFacingDirection);

            if (!game.GameBoard.IsWithinBounds(nextPosition))
            {
                throw new CustomException($"Cannot move {player.Username}: Robot would fall off the board.", 400);
            }

            if (game.GameBoard.HasWallBetween(player.CurrentPosition, nextPosition, player.CurrentFacingDirection))
            {
                throw new CustomException($"Cannot move {player.Username}: Wall is blocking the path.", 400);
            }

            var occupyingPlayer = allPlayers.FirstOrDefault(p => p.CurrentPosition == nextPosition && p.Username != player.Username);

            if (occupyingPlayer != null)
            {
                PushPlayer(occupyingPlayer, player.CurrentFacingDirection, game, allPlayers);
            }

            player.MoveTo(nextPosition);
        }
    }

    private void PushPlayer(Player.Player playerToPush, Direction pushDirection, Game game, List<Player.Player> allPlayers)
    {
        Position pushedPosition = GetNextPositionInDirection(playerToPush.CurrentPosition, pushDirection);

        if (!game.GameBoard.IsWithinBounds(pushedPosition))
        {
            throw new CustomException($"Cannot push {playerToPush.Username}: Robot would be pushed off the board.", 400);
        }

        if (game.GameBoard.HasWallBetween(playerToPush.CurrentPosition, pushedPosition, pushDirection))
        {
            throw new CustomException($"Cannot push {playerToPush.Username}: Wall is blocking the push.", 400);
        }

        var nextOccupyingPlayer = allPlayers.FirstOrDefault(p => p.CurrentPosition == pushedPosition && p.Username != playerToPush.Username);

        if (nextOccupyingPlayer != null)
        {
            PushPlayer(nextOccupyingPlayer, pushDirection, game, allPlayers);
        }

        playerToPush.MoveTo(pushedPosition);
    }

    private Position GetNextPositionInDirection(Position position, Direction direction)
    {
        return direction.DisplayName switch
        {
            "North" => new Position(position.X, position.Y - 1),
            "South" => new Position(position.X, position.Y + 1),
            "East" => new Position(position.X + 1, position.Y),
            "West" => new Position(position.X - 1, position.Y),
            _ => throw new CustomException($"Invalid direction: {direction.DisplayName}", 500)
        };
    }
}


using Roborally.core.domain.Game.Player;

namespace Roborally.core.domain.Game.CardActions;

public class MoveBackCardAction : ICardAction
{
    public void Execute(Player.Player player, Game game, List<Player.Player> players)
    {
        if (player == null) throw new CustomException("Player cannot be null.", 400);
        if (game == null) throw new CustomException("Game cannot be null.", 400);

        Position backwardPosition = player.GetPositionBehind();

        // Check if backward position is within board bounds
        if (!game.GameBoard.IsWithinBounds(backwardPosition))
        {
            throw new CustomException($"Cannot move {player.Username} backward: Robot would fall off the board.", 400);
        }

        // Get the opposite direction for wall checking (moving backward)
        Direction backwardDirection = GetOppositeDirection(player.CurrentFacingDirection);

        // Check for walls blocking the backward movement
        if (game.GameBoard.HasWallBetween(player.CurrentPosition, backwardPosition, backwardDirection))
        {
            throw new CustomException($"Cannot move {player.Username} backward: Wall is blocking the path.", 400);
        }

        // Check if the space is occupied by another player
        var occupyingPlayer = players.FirstOrDefault(p => p.CurrentPosition == backwardPosition && p.Username != player.Username);
        
        if (occupyingPlayer != null)
        {
            // Try to push the occupying player in the backward direction
            PushPlayer(occupyingPlayer, backwardDirection, game, players);
        }

        // Move the player backward
        player.MoveTo(backwardPosition);
        
        // Save this action as the last executed action for this player
        player.LastExecutedAction = this;
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

    private Direction GetOppositeDirection(Direction direction)
    {
        return direction.DisplayName switch
        {
            "North" => Direction.South,
            "South" => Direction.North,
            "East" => Direction.West,
            "West" => Direction.East,
            _ => throw new CustomException($"Invalid direction: {direction.DisplayName}", 500)
        };
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

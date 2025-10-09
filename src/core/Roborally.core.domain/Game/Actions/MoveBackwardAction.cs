using Roborally.core.domain.Bases;
using Roborally.core.domain.Game.Player;

namespace Roborally.core.domain.Game.Actions;

public class MoveBackwardAction : IAction
{
    private readonly int Spaces;
    
    public MoveBackwardAction(int spaces)
    {
        Spaces = spaces;
    }

    public void Execute(Player.Player player, Game game, List<Player.Player> players)
    {
        for (int i = 0; i < Spaces; i++)
        {
            Position backwardPosition = player.GetPositionBehind();

            // Use player's current direction for wall checking (walls are defined by forward direction)
            Direction oppositeDirection = GetOppositeDirection(player.CurrentFacingDirection);

            if (!game.GameBoard.IsWithinBounds(backwardPosition))
            {
                throw new CustomException($"Cannot move {player.Username} backward: Robot would fall off the board.", 400);
            }

            // Check for walls - walls are between current and backward position
            if (game.GameBoard.HasWallBetween(player.CurrentPosition, backwardPosition, oppositeDirection))
            {
                throw new CustomException($"Cannot move {player.Username} backward: Wall is blocking the path.", 400);
            }

            var occupyingPlayer = players.FirstOrDefault(p => 
                p.CurrentPosition == backwardPosition && p.Username != player.Username);
        
            if (occupyingPlayer != null)
            {
                PushPlayer(occupyingPlayer, oppositeDirection, game, players);
            }

            player.MoveTo(backwardPosition);
        }
    
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
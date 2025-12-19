using System.Linq;
using Roborally.core.domain;
using Roborally.core.domain.Game.Deck;
using Roborally.core.domain.Game.Player;

namespace Roborally.core.domain.Game.CardActions;

public class SwapPositionCardAction : ICardAction
{
/// <author name="Satish Gurung 2025-11-24 10:20:04 +0100 10" />
    public void Execute(Player.Player player, Game game, Bases.ISystemTime systemTime, CardExecutionContext? context = null)
    {
        if (context?.TargetPlayerUsername is null)
        {
            throw new CustomException("Swap Position card requires a target player username.", 400);
        }

        Player.Player? targetPlayer = game.Players.FirstOrDefault(p => p.Username == context.TargetPlayerUsername);

        if (targetPlayer is null)
        {
            throw new CustomException($"Target player {context.TargetPlayerUsername} does not exist.", 404);
        }

        if (targetPlayer.Username == player.Username)
        {
            throw new CustomException("Cannot swap positions with yourself.", 400);
        }

        var playerPosition = player.CurrentPosition;
        var targetPosition = targetPlayer.CurrentPosition;

        player.MoveTo(targetPosition);
        targetPlayer.MoveTo(playerPosition);

        player.RecordCardExecution(ProgrammingCard.SwapPosition, systemTime);
    }
}
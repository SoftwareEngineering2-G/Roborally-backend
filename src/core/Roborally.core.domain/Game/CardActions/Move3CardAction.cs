using Roborally.core.domain.Game.Deck;

namespace Roborally.core.domain.Game.CardActions;

public class Move3CardAction : ICardAction
{
/// <author name="Satish Gurung 2025-11-24 10:20:04 +0100 7" />
    public void Execute(Player.Player player, Game game, Bases.ISystemTime systemTime, CardExecutionContext? context = null)
    {
        for (int i = 0; i < 3; i++)
        {
            game.MovePlayerInDirection(player, player.CurrentFacingDirection);
        }
        player.RecordCardExecution(ProgrammingCard.Move3, systemTime);
    }
}
using Roborally.core.domain.Game.Deck;

namespace Roborally.core.domain.Game.CardActions;

public class RotateRightCardAction : ICardAction
{
/// <author name="Satish 2025-11-24 10:20:04 +0100 7" />
    public void Execute(Player.Player player, Game game, Bases.ISystemTime systemTime, CardExecutionContext? context = null)
    {
        player.RotateRight();
        player.RecordCardExecution(ProgrammingCard.RotateRight, systemTime);
    }
}
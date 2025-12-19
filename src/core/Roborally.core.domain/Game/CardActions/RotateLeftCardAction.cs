using Roborally.core.domain.Game.Deck;

namespace Roborally.core.domain.Game.CardActions;

public class RotateLeftCardAction : ICardAction
{
/// <author name="Satish Gurung 2025-11-24 10:20:04 +0100 7" />
    public void Execute(Player.Player player, Game game, Bases.ISystemTime systemTime, CardExecutionContext? context = null)
    {
        player.RotateLeft();
        player.RecordCardExecution(ProgrammingCard.RotateLeft, systemTime);
    }
}
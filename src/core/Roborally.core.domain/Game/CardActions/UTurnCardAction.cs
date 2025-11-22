using Roborally.core.domain.Game.Deck;

namespace Roborally.core.domain.Game.CardActions;

public class UTurnCardAction : ICardAction
{
    public void Execute(Player.Player player, Game game, Bases.ISystemTime systemTime, CardExecutionContext? context = null)
    {
        player.UTurn();
        player.RecordCardExecution(ProgrammingCard.UTurn, systemTime);
    }
}

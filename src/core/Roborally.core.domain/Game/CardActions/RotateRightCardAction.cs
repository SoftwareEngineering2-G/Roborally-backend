using Roborally.core.domain.Game.Deck;

namespace Roborally.core.domain.Game.CardActions;

public class RotateRightCardAction : ICardAction
{
    public void Execute(Player.Player player, Game game, Bases.ISystemTime systemTime)
    {
        player.RotateRight();
        player.RecordCardExecution(ProgrammingCard.RotateRight, systemTime);
    }
}

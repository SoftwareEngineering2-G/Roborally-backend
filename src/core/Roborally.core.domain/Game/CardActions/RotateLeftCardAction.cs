using Roborally.core.domain.Game.Deck;

namespace Roborally.core.domain.Game.CardActions;

public class RotateLeftCardAction : ICardAction
{
    public void Execute(Player.Player player, Game game, Bases.ISystemTime systemTime)
    {
        player.RotateLeft();
        player.RecordCardExecution(ProgrammingCard.RotateLeft, systemTime);
    }
}

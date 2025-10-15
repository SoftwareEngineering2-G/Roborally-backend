using Roborally.core.domain.Game.Deck;

namespace Roborally.core.domain.Game.CardActions;

public class Move3CardAction : ICardAction
{
    public void Execute(Player.Player player, Game game, Bases.ISystemTime systemTime)
    {
        for (int i = 0; i < 3; i++)
        {
            game.MovePlayerInDirection(player, player.CurrentFacingDirection);
        }
        player.RecordCardExecution(ProgrammingCard.Move3, systemTime);
    }
}

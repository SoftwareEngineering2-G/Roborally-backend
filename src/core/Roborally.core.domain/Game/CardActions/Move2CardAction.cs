using Roborally.core.domain.Game.Deck;

namespace Roborally.core.domain.Game.CardActions;

public class Move2CardAction : ICardAction {
    public void Execute(Player.Player player, Game game, Bases.ISystemTime systemTime, CardExecutionContext? context = null) {
        for (int i = 0; i < 2; i++) {
            game.MovePlayerInDirection(player, player.CurrentFacingDirection);
        }
        player.RecordCardExecution(ProgrammingCard.Move2, systemTime);
    }
}

using Roborally.core.domain.Game.Deck;

namespace Roborally.core.domain.Game.CardActions;

public class Move1CardAction : ICardAction {
    public void Execute(Player.Player player, Game game, Bases.ISystemTime systemTime) {
        game.MovePlayerInDirection(player, player.CurrentFacingDirection);
        player.RecordCardExecution(ProgrammingCard.Move1, systemTime);
    }
}

namespace Roborally.core.domain.Game.CardActions;

public class UTurnCardAction : ICardAction
{
    public void Execute(Player.Player player, Game game, List<Player.Player> players)
    {
        if (player == null) throw new CustomException("Player cannot be null.", 400);
        
        player.UTurn();
        
        // Save this action as the last executed action for this player
        player.LastExecutedAction = this;
    }
}

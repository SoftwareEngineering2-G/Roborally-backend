using Roborally.core.domain.Bases;

namespace Roborally.core.domain.Game.Actions;

public class RotateLeftAction : IAction
{
    public void Execute(Player.Player player, Game game, List<Player.Player> players)
    {
        if (player == null) throw new CustomException("Player cannot be null.", 400);
        
        player.RotateLeft();
        
        // Save this action as the last executed action for this player
        player.LastExecutedAction = this;
    }
}

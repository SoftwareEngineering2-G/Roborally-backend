using Roborally.core.domain.Bases;

namespace Roborally.core.domain.Game.Actions;

public class AgainAction : IAction
{
    public void Execute(Player.Player player, Game game, List<Player.Player> players)
    {
        if (player == null) throw new CustomException("Player cannot be null.", 400);
        
        // Retrieve the last executed action for this player
        if (player.LastExecutedAction == null)
        {
            throw new CustomException($"Cannot execute Again card for {player.Username}: No previous action to repeat.", 400);
        }
        
        // Check if the last action was also an Again action to prevent infinite loops
        if (player.LastExecutedAction is AgainAction)
        {
            throw new CustomException($"Cannot execute Again card for {player.Username}: Cannot repeat an Again action.", 400);
        }
        
        // Execute the last action again
        player.LastExecutedAction.Execute(player, game, players);

        // Note: We don't update LastExecutedAction here because Again should not replace the last action
        // The repeated action will update it if needed
    }
}

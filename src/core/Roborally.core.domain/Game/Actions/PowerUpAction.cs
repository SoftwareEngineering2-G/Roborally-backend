namespace Roborally.core.domain.Game.Actions;

public class PowerUpAction : IAction
{
    public void Execute(Player.Player player, Game game, List<Player.Player> players)
    {
        if (player == null) throw new CustomException("Player cannot be null.", 400);
        
        // PowerUp card increases the player's energy or gives special abilities
        // This is a placeholder implementation - adjust based on your game rules
        // For now, this does nothing as the PowerUp mechanic may need additional player properties
        
        // TODO: Implement PowerUp logic when energy/power system is defined
        // Example: player.Energy += 1;

        // Save this action as the last executed action for this player
        player.LastExecutedAction = this;
    }
}

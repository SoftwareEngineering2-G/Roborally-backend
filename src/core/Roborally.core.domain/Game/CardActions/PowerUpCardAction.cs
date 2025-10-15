using Roborally.core.domain.Game.Deck;

namespace Roborally.core.domain.Game.CardActions;

public class PowerUpCardAction : ICardAction
{
    public void Execute(Player.Player player, Game game, Bases.ISystemTime systemTime)
    {
        // PowerUp card increases the player's energy or gives special abilities
        // This is a placeholder implementation - adjust based on your game rules
        // For now, this does nothing as the PowerUp mechanic may need additional player properties
        
        // TODO: Implement PowerUp logic when energy/power system is defined
        // Example: player.Energy += 1;

        player.RecordCardExecution(ProgrammingCard.PowerUp, systemTime);
    }
}

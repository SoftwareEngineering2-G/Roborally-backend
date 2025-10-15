using Roborally.core.domain.Game.Deck;

namespace Roborally.core.domain.Game.CardActions;

public class AgainCardAction : ICardAction
{
    public void Execute(Player.Player player, Game game, Bases.ISystemTime systemTime)
    {
        // Check if there's a last executed card
        var lastExecutedCard = player.GetLastExecutedCard();
        
        if (lastExecutedCard is null)
        {
            throw new CustomException($"Cannot execute Again card for {player.Username}: No previous action to repeat.", 400);
        }
        
        // Check if the last action was also an Again action to prevent infinite loops
        if (lastExecutedCard == ProgrammingCard.Again)
        {
            throw new CustomException($"Cannot execute Again card for {player.Username}: Cannot repeat an Again action.", 400);
        }
        
        // Reconstruct the action from the card
        var actionToRepeat = ActionFactory.CreateAction(lastExecutedCard);
        
        // Execute the last action again
        actionToRepeat.Execute(player, game, systemTime);

        // Note: We don't record the Again action itself, only the repeated action
    }
}

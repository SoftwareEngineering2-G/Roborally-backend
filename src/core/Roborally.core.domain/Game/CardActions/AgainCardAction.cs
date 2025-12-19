using Roborally.core.domain.Game.Deck;

namespace Roborally.core.domain.Game.CardActions;

public class AgainCardAction : ICardAction
{
/// <author name="Satish 2025-11-24 10:20:04 +0100 7" />
    public void Execute(Player.Player player, Game game, Bases.ISystemTime systemTime, CardExecutionContext? context = null)
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

        // Interactive cards require user input that wasn't stored with the original execution,
        // so they cannot be repeated by the Again card
        if (lastExecutedCard == ProgrammingCard.SwapPosition || lastExecutedCard == ProgrammingCard.MovementChoice)
        {
            throw new CustomException($"Cannot execute Again card for {player.Username}: Cannot repeat interactive card {lastExecutedCard.DisplayName}.", 400);
        }
        
        // Reconstruct the action from the card
        var actionToRepeat = ActionFactory.CreateAction(lastExecutedCard);
        
        // Execute the last action again
        actionToRepeat.Execute(player, game, systemTime);

        // Note: We don't record the Again action itself, only the repeated action
    }
}
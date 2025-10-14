using Roborally.core.domain.Game.Deck;

namespace Roborally.core.domain.Game.CardActions;

public class AgainCardAction : ICardAction
{
    public void Execute(Player.Player player, Game game, List<Player.Player> players)
    {
        if (player == null) throw new CustomException("Player cannot be null.", 400);
        
        // Check if there's a last executed card name
        if (string.IsNullOrEmpty(player.LastExecutedCardName))
        {
            throw new CustomException($"Cannot execute Again card for {player.Username}: No previous action to repeat.", 400);
        }
        
        // Check if the last action was also an Again action to prevent infinite loops
        if (player.LastExecutedCardName == "Again")
        {
            throw new CustomException($"Cannot execute Again card for {player.Username}: Cannot repeat an Again action.", 400);
        }
        
        // Reconstruct the action from the card name
        var lastCard = ProgrammingCard.FromString(player.LastExecutedCardName);
        var actionToRepeat = ActionFactory.CreateAction(lastCard);
        
        // Execute the last action again
        actionToRepeat.Execute(player, game, players);

        // Note: We don't update LastExecutedCardName here because Again should not replace the last action
    }
}

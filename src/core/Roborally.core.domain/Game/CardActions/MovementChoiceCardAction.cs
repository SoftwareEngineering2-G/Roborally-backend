using System.Linq;
using Roborally.core.domain;
using Roborally.core.domain.Game.Deck;

namespace Roborally.core.domain.Game.CardActions;

public class MovementChoiceCardAction : ICardAction
{
    private static readonly ProgrammingCard[] AllowedChoices = new[]
    {
        ProgrammingCard.Move1,
        ProgrammingCard.Move2,
        ProgrammingCard.Move3,
        ProgrammingCard.MoveBack
    };

    public void Execute(Player.Player player, Game game, Bases.ISystemTime systemTime, CardExecutionContext? context = null)
    {
        var chosenCard = context?.SelectedMovementCard;

        if (chosenCard is null)
        {
            throw new CustomException("Movement Choice card requires a selected movement option.", 400);
        }

        if (!AllowedChoices.Contains(chosenCard))
        {
            throw new CustomException($"Selected card {chosenCard.DisplayName} is not a valid movement option.", 400);
        }

        if (chosenCard == ProgrammingCard.MoveBack)
        {
            game.MovePlayerInDirection(player, player.CurrentFacingDirection.Opposite());
        }
        else
        {
            int spaces = chosenCard == ProgrammingCard.Move3 ? 3 : chosenCard == ProgrammingCard.Move2 ? 2 : 1;

            for (int i = 0; i < spaces; i++)
            {
                game.MovePlayerInDirection(player, player.CurrentFacingDirection);
            }
        }

        player.RecordCardExecution(ProgrammingCard.MovementChoice, systemTime);
    }
}

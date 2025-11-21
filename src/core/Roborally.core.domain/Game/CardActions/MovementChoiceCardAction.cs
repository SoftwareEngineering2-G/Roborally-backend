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
        ProgrammingCard.MoveBack,
        ProgrammingCard.RotateLeft,
        ProgrammingCard.RotateRight,
        ProgrammingCard.UTurn
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

        switch (chosenCard.DisplayName)
        {
            case "Move 1":
                game.MovePlayerInDirection(player, player.CurrentFacingDirection);
                break;
            case "Move 2":
                for (int i = 0; i < 2; i++) {
                    game.MovePlayerInDirection(player, player.CurrentFacingDirection);
                }
                break;
            case "Move 3":
                for (int i = 0; i < 3; i++) {
                    game.MovePlayerInDirection(player, player.CurrentFacingDirection);
                }
                break;
            case "Move Back":
                game.MovePlayerInDirection(player, player.CurrentFacingDirection.Opposite());
                break;
            case "Rotate Left":
                player.RotateLeft();
                break;
            case "Rotate Right":
                player.RotateRight();
                break;
            case "U-Turn":
                player.UTurn();
                break;
        }

        player.RecordCardExecution(ProgrammingCard.MovementChoice, systemTime);
    }
}

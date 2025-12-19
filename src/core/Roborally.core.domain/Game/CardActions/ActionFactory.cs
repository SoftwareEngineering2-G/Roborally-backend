using Roborally.core.domain.Game.Deck;

namespace Roborally.core.domain.Game.CardActions;

public static class ActionFactory
{
/// <author name="Nilanjana Devkota 2025-10-14 19:37:00 +0200 7" />
    public static ICardAction CreateAction(ProgrammingCard card)
    {
        
        return card.DisplayName switch
        {
            "Move 1" => new Move1CardAction(),
            "Move 2" => new Move2CardAction(),
            "Move 3" => new Move3CardAction(),
            "Rotate Left" => new RotateLeftCardAction(),
            "Rotate Right" => new RotateRightCardAction(),
            "U-Turn" => new UTurnCardAction(),
            "Move Back" => new MoveBackwardCardAction(1),
            "Again" => new AgainCardAction(),
            "Swap Position" => new SwapPositionCardAction(),
            "Movement Choice" => new MovementChoiceCardAction(),
            _ => throw new CustomException($"Unknown card type: {card.DisplayName}", 400)
        };
    }
}
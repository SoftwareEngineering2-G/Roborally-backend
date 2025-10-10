using Roborally.core.domain.Deck;

namespace Roborally.core.domain.Game.Actions;

public static class ActionFactory
{
    public static IAction CreateAction(ProgrammingCard card)
    {
        return card.DisplayName switch
        {
            "Move 1" => new Move1Action(),
            "Move 2" => new Move2Action(),
            "Move 3" => new Move3Action(),
            "Rotate Left" => new RotateLeftAction(),
            "Rotate Right" => new RotateRightAction(),
            "U-Turn" => new UTurnAction(),
            "Move Back" => new MoveBackwardAction(1),
            "Power Up" => new PowerUpAction(),
            "Again" => new AgainAction(),
            _ => throw new CustomException($"Unknown card type: {card.DisplayName}", 400)
        };
    }
}

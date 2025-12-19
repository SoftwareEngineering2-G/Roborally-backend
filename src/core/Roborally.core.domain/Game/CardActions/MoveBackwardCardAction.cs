using Roborally.core.domain.Game.Deck;

namespace Roborally.core.domain.Game.CardActions;

public class MoveBackwardCardAction : ICardAction
{
    private readonly int _spaces;
    
/// <author name="nilanjanadevkota 2025-10-14 19:37:00 +0200 9" />
    public MoveBackwardCardAction(int spaces)
    {
        _spaces = spaces;
    }

/// <author name="Satish 2025-11-24 10:20:04 +0100 14" />
    public void Execute(Player.Player player, Game game, Bases.ISystemTime systemTime, CardExecutionContext? context = null)
    {
        for (int i = 0; i < _spaces; i++)
        {
            game.MovePlayerInDirection(player, player.CurrentFacingDirection.Opposite());
        }
        player.RecordCardExecution(ProgrammingCard.MoveBack, systemTime);
    }
}
using Roborally.core.domain.Game.Deck;

namespace Roborally.core.domain.Game.CardActions;

public class MoveBackwardCardAction : ICardAction
{
    private readonly int _spaces;
    
    public MoveBackwardCardAction(int spaces)
    {
        _spaces = spaces;
    }

    public void Execute(Player.Player player, Game game, Bases.ISystemTime systemTime)
    {
        for (int i = 0; i < _spaces; i++)
        {
            game.MovePlayerInDirection(player, player.CurrentFacingDirection.Opposite(), shouldPush: true);
            game.CheckPlayerOnCheckpoint(player, systemTime); // Check after each step
        }
        player.RecordCardExecution(ProgrammingCard.MoveBack, systemTime);
    }
}
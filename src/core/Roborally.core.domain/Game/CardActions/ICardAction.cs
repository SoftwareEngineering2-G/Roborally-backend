using Roborally.core.domain.Bases;

namespace Roborally.core.domain.Game.CardActions;

public interface ICardAction
{
    void Execute(Player.Player player, Game game, ISystemTime systemTime);
}
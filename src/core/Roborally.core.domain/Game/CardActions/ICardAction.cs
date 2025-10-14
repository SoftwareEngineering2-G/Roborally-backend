
namespace Roborally.core.domain.Game.CardActions;

public interface ICardAction
{
    void Execute(Player.Player player, Game game, List<Player.Player> players);
}
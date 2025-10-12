using Roborally.core.domain.Game.Player;

namespace Roborally.core.domain.Game.Actions;

public interface IAction
{
    void Execute(Player.Player player, Game game, List<Player.Player> players);
}


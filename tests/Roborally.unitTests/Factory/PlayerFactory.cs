using Roborally.core.domain.Game.Player;

namespace Roborally.unitTests.Factory;

public class PlayerFactory
{
    public static Player GetValidPlayer()
    {
        return new Player("Alice", Guid.NewGuid(), new Position(0, 0), Robot.Black);
    }

    public static List<Player> GetValidPlayers()
    {
        return new List<Player>()
            { GetValidPlayer(), new Player("Bob", Guid.NewGuid(), new Position(1, 1), Robot.Blue) };
    }
}
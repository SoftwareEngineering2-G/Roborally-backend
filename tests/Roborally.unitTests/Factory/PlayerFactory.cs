using Roborally.core.domain.Game.Player;
using Roborally.core.domain.User;

namespace Roborally.unitTests.Factory;

public static class PlayerFactory
{
    private static int _playerId = 1;

    public static Player GetValidPlayer()
    {
        var user = new User
        {
            Username = "User" + _playerId++,
            Password = "SecurePass123",
            Birthday = new DateOnly(1990, 1, 1)
        };

        var player = new Player(user.Username, Guid.NewGuid(), new Position(0, 0), Robot.Black)
        {
            User = user
        };

        return player;
    }

    public static List<Player> GetValidPlayers(int playersCount)
    {
        return Enumerable.Range(0, playersCount)
            .Select(_ => GetValidPlayer())
            .ToList();
    }
}
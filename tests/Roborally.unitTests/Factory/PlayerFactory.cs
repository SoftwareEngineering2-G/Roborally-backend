using Roborally.core.domain.Game.Player;
using Roborally.core.domain.User;

namespace Roborally.unitTests.Factory;

public static class PlayerFactory
{
    private static int _playerId = 1;

    public static Player GetValidPlayer(DateOnly? birthday = null)
    {
        var user = new User
        {
            Username = "User" + _playerId++,
            Password = "SecurePass123",
            Birthday = birthday ?? DateOnly.FromDateTime(DateTime.Now.AddYears(-20))
        };

        var player = new Player(user.Username, Guid.NewGuid(), new Position(0, 0), Robot.Black)
        {
            User = user
        };

        return player;
    }

    public static List<Player> GetValidPlayers(int playersCount)
    {
        var birthday = DateOnly.FromDateTime(DateTime.Now.AddYears(-20));

        return Enumerable.Range(0, playersCount)
            .Select(_ =>
            {
                var player = GetValidPlayer(birthday);
                birthday = birthday.AddDays(1);
                return player;
            }).ToList();
    }
}
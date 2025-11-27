using Roborally.core.domain.Game;
using Roborally.core.domain.Game.Gameboard;
using Roborally.core.domain.Game.Player;
using Roborally.core.domain.Lobby;

namespace Roborally.unitTests.Factory;

public static class GameFactory
{
    public static Game GetValidGame(int playersCount = 2)
    {
        var players = PlayerFactory.GetValidPlayers(playersCount);
        return new Game(
            Guid.NewGuid(),
            players[0].Username,
            "Test Game",
            players,
            GameBoardFactory.GetEmptyBoard(),
            false,
            DateTime.Now
        );
    }

    public static Game GetGameFromLobby(GameLobby lobby)
    {
        return new Game(
            lobby.GameId,
            lobby.HostUsername,
            lobby.Name,
            lobby.JoinedUsers.Select(user => new Player(user.Username, lobby.GameId, new Position(0, 0), Robot.Red))
                .ToList(),
            GameBoardFactory.GetEmptyBoard(),
            lobby.IsPrivate,
            DateTime.Now
        );
    }
}
using Roborally.core.domain.Game;
using Roborally.core.domain.Game.Gameboard;

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
}
using Roborally.core.domain.Game;
using Roborally.core.domain.Game.Gameboard;

namespace Roborally.unitTests.Factory;

public class GameFactory
{
    public static Game GetValidGame()
    {
        var players = PlayerFactory.GetValidPlayers();
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
using FastEndpoints;
using Roborally.core.application.CommandContracts;
using Roborally.core.domain.Game;
using Roborally.core.domain.User;

namespace Roborally.core.application.CommandHandlers;

public class GetGameCommandHandler:ICommandHandler<GetGameCommand, Game>
{
    //TODO change from the dummy data to real data from the database
    public async Task<Game> ExecuteAsync(GetGameCommand command, CancellationToken ct)
    {
        GameBoard board = GameBoard.CreateEmpty(12, 12);
        Player player1 = new Player()
        {
            User = new User { Username = "Player1", Password = "password", Birthday = DateOnly.FromDateTime(DateTime.Now) },
            Robot = Robot.BLUE,
            CurrentFacingDirection = Direction.North,
            CurrentPosition = new Position { X = 1, Y = 1 }
        };
        Game game = new Game()
        {
            GameBoard = board,
            GamePhase = GamePhase.ProgrammingPhase,
        };
        game.AddPlayer(player1);
        return game;
    }
}
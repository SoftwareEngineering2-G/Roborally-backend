using FastEndpoints;
using Roborally.core.application.CommandContracts.Game;
using Roborally.core.domain.Game;

namespace Roborally.core.application.CommandHandlers.Game;

public class
    GetCurrentGameStateCommandHandler : ICommandHandler<GetCurrentGameStateCommand,
    GetCurrentGameStateCommandResponse> {
    private readonly IGameRepository _gameRepository;


    public GetCurrentGameStateCommandHandler(IGameRepository gameRepository) {
        _gameRepository = gameRepository;
    }


    public async Task<GetCurrentGameStateCommandResponse> ExecuteAsync(GetCurrentGameStateCommand command,
        CancellationToken ct) {
        domain.Game.Game? game = await _gameRepository.FindAsync(command.GameId, ct);

        if (game is null) {
            throw new CustomException("Game does not exist", 404);
        }

        return new GetCurrentGameStateCommandResponse() {
            GameId = game.GameId.ToString(),
            HostUsername = game.HostUsername,
            Name = game.Name,
            CurrentPhase = game.CurrentPhase.DisplayName,
            GameBoard = new GetCurrentGameStateCommandResponse.GameBoardSpaces(game.GameBoard.Name,
                game.GameBoard.Space.Select(row =>
                        row.Select(space => new GetCurrentGameStateCommandResponse.Space(space.Name())).ToArray())
                    .ToArray()),
            Players = game.Players
                .Select(p => new GetCurrentGameStateCommandResponse.Player(p.Username, p.Robot.DisplayName)).ToList()
        };
    }
}
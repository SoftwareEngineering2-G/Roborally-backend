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
            GameBoard = game.GameBoard,
            Players = game.Players.Select(player => new GetCurrentGameStateCommandResponse.Player(
                player.Username,
                player.Robot.DisplayName,
                player.CurrentFacingDirection.ToString(),
                player.CurrentPosition is null
                    ? null
                    : new GetCurrentGameStateCommandResponse.Position(player.CurrentPosition.X,
                        player.CurrentPosition.Y)
            )).ToList()
        };
    }
}
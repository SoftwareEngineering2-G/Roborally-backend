using FastEndpoints;
using Roborally.core.application.CommandContracts;
using Roborally.core.domain.Game;

namespace Roborally.core.application.CommandHandlers;

public class GetGameCommandHandler:ICommandHandler<GetGameCommand,GetGameCommandResponse>
{
    private readonly IGameRepository _gameRepository;

    public GetGameCommandHandler(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }

    public async Task<GetGameCommandResponse> ExecuteAsync(GetGameCommand command, CancellationToken ct)
    {
        domain.Game.Game? game = await _gameRepository.FindAsync(command.GameId,ct);
        if (game is null)
        {
            throw new CustomException("Game not found", 404);
        }

        return new GetGameCommandResponse()
        {
            Game = game
        };
    }
}
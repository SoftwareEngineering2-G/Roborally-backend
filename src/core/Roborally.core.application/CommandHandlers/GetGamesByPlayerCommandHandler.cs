using FastEndpoints;
using Roborally.core.application.CommandContracts;
using Roborally.core.domain.Game;

namespace Roborally.core.application.CommandHandlers;

public class GetGamesByPlayerCommandHandler: ICommandHandler<GetGamesByPlayerCommand, List<Roborally.core.domain.Game.Game>>
{
    private readonly IGameRepository _gameRepository;
    public GetGamesByPlayerCommandHandler(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }
    public async Task<List<Game>> ExecuteAsync(GetGamesByPlayerCommand command, CancellationToken ct)
    {
        List<Game> games = await _gameRepository.ListAllByPlayerIdAsync(command.PlayerId, ct);
        return games;
    }
}
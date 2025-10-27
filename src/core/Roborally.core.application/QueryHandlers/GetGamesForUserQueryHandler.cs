using FastEndpoints;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.QueryContracts;

namespace Roborally.core.application.QueryHandlers;

public class GetGamesForUserQueryHandler : ICommandHandler<GetGamesForUserQuery, List<GetGamesForUserResponse>> {

    private readonly IGameRepository _gameRepository;


    public GetGamesForUserQueryHandler(IGameRepository gameRepository) {
        _gameRepository = gameRepository;
    }

    public Task<List<GetGamesForUserResponse>> ExecuteAsync(GetGamesForUserQuery query, CancellationToken ct) {
        return _gameRepository.QueryGamesForUserAsync(query, ct);
    }
}
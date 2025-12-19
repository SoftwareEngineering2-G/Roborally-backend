using FastEndpoints;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.QueryContracts;

namespace Roborally.core.application.QueryHandlers;

public class GetGamesForUserQueryHandler : ICommandHandler<GetGamesForUserQuery, GetGamesForUserQueryResult> {

    private readonly IGameRepository _gameRepository;


/// <author name="Sachin Baral 2025-10-27 17:05:05 +0100 12" />
    public GetGamesForUserQueryHandler(IGameRepository gameRepository) {
        _gameRepository = gameRepository;
    }

/// <author name="Sachin Baral 2025-11-15 19:46:26 +0100 16" />
    public Task<GetGamesForUserQueryResult> ExecuteAsync(GetGamesForUserQuery query, CancellationToken ct) {
        return _gameRepository.QueryGamesForUserAsync(query, ct);
    }
}
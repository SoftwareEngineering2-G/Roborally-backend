using FastEndpoints;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.QueryContracts;

namespace Roborally.core.application.QueryHandlers;

public class GetUserCurrentPlayingStatusQueryHandler : ICommandHandler<GetUserCurrentPlayingStatusQuery, GetCurrentUserPlayingStatusResponse> {

    private readonly IGameRepository _gameRepository;


/// <author name="Sachin Baral 2025-10-27 17:05:05 +0100 12" />
    public GetUserCurrentPlayingStatusQueryHandler(IGameRepository gameRepository) {
        _gameRepository = gameRepository;
    }


/// <author name="Sachin Baral 2025-10-27 17:05:05 +0100 17" />
    public Task<GetCurrentUserPlayingStatusResponse> ExecuteAsync(GetUserCurrentPlayingStatusQuery command, CancellationToken ct) {
        return _gameRepository.QueryUserCurrentPlayingStatusAsync(command, ct);
    }
}
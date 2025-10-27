using FastEndpoints;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.QueryContracts;

namespace Roborally.core.application.QueryHandlers;

public class GetUserCurrentPlayingStatusQueryHandler : ICommandHandler<GetUserCurrentPlayingStatusQuery, GetCurrentUserPlayingStatusResponse> {

    private readonly IGameRepository _gameRepository;


    public GetUserCurrentPlayingStatusQueryHandler(IGameRepository gameRepository) {
        _gameRepository = gameRepository;
    }


    public Task<GetCurrentUserPlayingStatusResponse> ExecuteAsync(GetUserCurrentPlayingStatusQuery command, CancellationToken ct) {
        return _gameRepository.QueryUserCurrentPlayingStatusAsync(command, ct);
    }
}
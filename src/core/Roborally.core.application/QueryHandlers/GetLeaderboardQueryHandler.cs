using FastEndpoints;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.QueryContracts;

namespace Roborally.core.application.QueryHandlers;

public class GetLeaderboardQueryHandler : ICommandHandler<GetLeaderboardQuery, GetLeaderboardQueryResult>{

    private readonly IUserRepository _userRepository;
    public GetLeaderboardQueryHandler(IUserRepository userRepository) {
        _userRepository = userRepository;
    }

    public Task<GetLeaderboardQueryResult> ExecuteAsync(GetLeaderboardQuery command, CancellationToken ct) {
        return _userRepository.GetLeaderboardQueryAsync(command, ct);
    }
}
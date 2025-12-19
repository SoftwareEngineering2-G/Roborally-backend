using FastEndpoints;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.QueryContracts;

namespace Roborally.core.application.QueryHandlers;

public class GetLeaderboardQueryHandler : ICommandHandler<GetLeaderboardQuery, GetLeaderboardQueryResult>{

    private readonly IUserRepository _userRepository;
/// <author name="Sachin Baral 2025-11-15 19:46:26 +0100 10" />
    public GetLeaderboardQueryHandler(IUserRepository userRepository) {
        _userRepository = userRepository;
    }

/// <author name="Sachin Baral 2025-11-15 19:46:26 +0100 14" />
    public Task<GetLeaderboardQueryResult> ExecuteAsync(GetLeaderboardQuery command, CancellationToken ct) {
        return _userRepository.GetLeaderboardQueryAsync(command, ct);
    }
}
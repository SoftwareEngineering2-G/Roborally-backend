using FastEndpoints;
using Roborally.core.domain.Lobby;

namespace Roborally.core.application.CommandContracts;

public class GetUserPausedGamesCommand : ICommand<IList<GetUserPausedGamesResponse>>
{
    public required string Username { get; init; }
}

public class GetUserPausedGamesResponse {
    public Guid GameId { get; init; }
    public string Name { get; init; }
    public string HostUsername { get; init; }
    public string[] PlayerUsernames { get; init; }
}
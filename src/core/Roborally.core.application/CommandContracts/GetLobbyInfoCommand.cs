using FastEndpoints;

namespace Roborally.core.application.CommandContracts;

public class GetLobbyInfoCommand : ICommand<GetLobbyInfoCommandResponse> {
    public required string Username { get; init; }
    public required Guid GameId { get; init; }
} 

public class GetLobbyInfoCommandResponse {
    public Guid GameId { get; init; }

    public string Lobbyname { get; init; }
    public List<string> JoinedUsernames { get; init; }
    public string HostUsername { get; init; }
    public List<string> RequiredUsernames { get; init; }
    public string? PausedGameBoardName { get; init; }
}
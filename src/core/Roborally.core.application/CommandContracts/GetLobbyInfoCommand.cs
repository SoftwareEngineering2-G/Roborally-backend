using FastEndpoints;

namespace Roborally.core.application.CommandContracts;

public class GetLobbyInfoCommand : ICommand<GetLobbyInfoCommandResponse> {
    public string Username { get; set; }
    public Guid GameId { get; set; }
} 

public class GetLobbyInfoCommandResponse {
    public Guid GameId { get; set; }

    public string Lobbyname { get; set; }
    public List<string> JoinedUsernames { get; set; }
    public string HostUsername { get; set; }
}
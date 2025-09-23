using FastEndpoints;
using Roborally.core.application.CommandContracts;

namespace Roborally.webapi.RestEndpoints.Lobby;

public class GetLobbyInfoEndpoint : Endpoint<GetLobbyInfoRequest, GetLobbyInfoResponse> {
    public override void Configure() {
        Get("/game-lobbies/{GameId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetLobbyInfoRequest req, CancellationToken ct) {
        GetLobbyInfoCommand command = new GetLobbyInfoCommand() {
            GameId = req.GameId,
            Username = req.Username
        };

        GetLobbyInfoCommandResponse response = await command.ExecuteAsync(ct);
        await Send.OkAsync(new GetLobbyInfoResponse() {
            GameId = response.GameId,
            JoinedUsernames = response.JoinedUsernames,
            HostUsername = response.HostUsername,
            Lobbyname = response.Lobbyname,
        }, ct);
    }
}

public class GetLobbyInfoRequest {
    public string Username { get; set; }
    public Guid GameId { get; set; }
}

public class GetLobbyInfoResponse {
    public Guid GameId { get; set; }

    public string Lobbyname { get; set; }
    public List<string> JoinedUsernames { get; set; }
    public string HostUsername { get; set; }
}
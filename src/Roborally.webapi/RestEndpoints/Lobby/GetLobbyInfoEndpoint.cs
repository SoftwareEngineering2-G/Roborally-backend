using FastEndpoints;
using Roborally.core.application.CommandContracts;

namespace Roborally.webapi.RestEndpoints.Lobby;

public class GetLobbyInfoEndpoint : Endpoint<GetLobbyInfoRequest, GetLobbyInfoResponse> {
/// <author name="Sachin Baral 2025-09-20 20:52:08 +0200 7" />
    public override void Configure() {
        Get("/game-lobbies/{GameId}");
        AllowAnonymous();
    }

/// <author name="Sachin Baral 2025-09-20 20:52:08 +0200 12" />
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
            RequiredUsernames = response.RequiredUsernames,
            PausedGameBoardName = response.PausedGameBoardName
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
    public List<string> RequiredUsernames { get; set; }
    public string? PausedGameBoardName { get; set; }
}
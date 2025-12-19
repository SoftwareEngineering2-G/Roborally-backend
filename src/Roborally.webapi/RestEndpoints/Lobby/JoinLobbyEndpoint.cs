using FastEndpoints;
using Roborally.core.application.CommandContracts;

namespace Roborally.webapi.RestEndpoints.Lobby;

public class JoinLobbyEndpoint : Endpoint<JoinLobbyRequest> {
/// <author name="Sachin Baral 2025-09-19 13:01:07 +0200 7" />
    public override void Configure() {
        Post("/game-lobbies/{gameId}/join");
        Summary(s => {
            s.Summary = "Join a game lobby";
            s.Description = "Allows a user to join an existing game lobby";
            s.Response(200, "Successfully joined the lobby");
            s.Response(400, "Bad request - lobby full or game already started");
            s.Response(404, "User or lobby not found");
        });
    }

/// <author name="Sachin Baral 2025-09-19 13:01:07 +0200 18" />
    public override async Task HandleAsync(JoinLobbyRequest req, CancellationToken ct) {
        JoinLobbyCommand command = new JoinLobbyCommand() {
            Username = req.Username,
            GameId = req.GameId
        };

        await command.ExecuteAsync(ct);
        await Send.OkAsync(cancellation: ct);
    }
}

public class JoinLobbyRequest {
    public Guid GameId { get; set; }
    public string Username { get; set; }
}
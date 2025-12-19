using FastEndpoints;
using Roborally.core.application.CommandContracts;

namespace Roborally.webapi.RestEndpoints.Lobby;

public class JoinContinueLobbyEndpoint : Endpoint<JoinContinueLobbyRequest> {
/// <author name="Truong Son NGO 2025-11-12 15:35:28 +0100 7" />
    public override void Configure() {
        Post("/game-lobbies/{gameId}/join-continue");
        Summary(s => {
            s.Summary = "Join a paused game lobby";
            s.Description = "Allows a user to join a paused game lobby to continue playing";
            s.Response(200, "Successfully joined the lobby");
            s.Response(400, "Bad request - lobby full or game already started");
            s.Response(404, "User or lobby not found");
        });
    }

/// <author name="Truong Son NGO 2025-11-12 15:35:28 +0100 18" />
    public override async Task HandleAsync(JoinContinueLobbyRequest req, CancellationToken ct) {
        JoinContinueLobbyCommand command = new JoinContinueLobbyCommand() {
            Username = req.Username,
            GameId = req.GameId
        };

        await command.ExecuteAsync(ct);
        await Send.OkAsync(cancellation: ct);
    }
}

public class JoinContinueLobbyRequest {
    public Guid GameId { get; set; }
    public string Username { get; set; }
}
using FastEndpoints;
using Roborally.core.application.CommandContracts;

namespace Roborally.webapi.RestEndpoints.Lobby;

public class ContinueGameEndpoint : Endpoint<ContinueGameRequest> {
/// <author name="Truong Son NGO 2025-11-12 15:35:28 +0100 7" />
    public override void Configure() {
        Post("/game-lobbies/{gameId}/continue");
    }


/// <author name="Truong Son NGO 2025-11-12 15:35:28 +0100 12" />
    public override async Task HandleAsync(ContinueGameRequest req, CancellationToken ct) {
        ContinueGameCommand command = new ContinueGameCommand() {
            GameId = req.GameId,
            Username = req.Username,
        };

        await command.ExecuteAsync(ct);

        await Send.OkAsync(cancellation:ct);
    }
}

public class ContinueGameRequest {
    public string Username { get; set; }
    public Guid GameId { get; set; }
}
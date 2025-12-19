using FastEndpoints;
using Roborally.core.application.CommandContracts.Game;

namespace Roborally.webapi.RestEndpoints.Game.Commands;

public class RequestPauseGameEndpoint : Endpoint<RequestGamePauseRequest> {
/// <author name="Truong Son NGO 2025-11-12 15:35:28 +0100 7" />
    public override void Configure() {
        Post("/games/{gameId}/pause/request");
    }

/// <author name="Truong Son NGO 2025-11-12 15:35:28 +0100 11" />
    public override async Task HandleAsync(RequestGamePauseRequest req, CancellationToken ct) {
        RequestPauseGameCommand command = new RequestPauseGameCommand() {
            GameId = req.GameId,
            RequesterUsername = req.Username
        };

        await command.ExecuteAsync(ct);
        await Send.OkAsync(cancellation: ct);
    }
}

public class RequestGamePauseRequest {
    public Guid GameId { get; set; }
    public required string Username { get; set; }
}
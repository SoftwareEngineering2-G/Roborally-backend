using FastEndpoints;
using Roborally.core.application.CommandContracts.Game;

namespace Roborally.webapi.RestEndpoints.Game.Commands;

public class ResponsePauseGameEndpoint : Endpoint<ResponseGamePauseRequest> {
/// <author name="Truong Son NGO 2025-11-12 15:35:28 +0100 7" />
    public override void Configure() {
        Post("/games/{gameId}/pause/respond");
    }

/// <author name="Truong Son NGO 2025-11-12 15:35:28 +0100 11" />
    public override async Task HandleAsync(ResponseGamePauseRequest req, CancellationToken ct) {
        ResponsePauseGameCommand command = new ResponsePauseGameCommand() {
            GameId = req.GameId,
            ResponderUsername = req.Username,
            Approved = req.Approved,
        };

        await command.ExecuteAsync(ct);
        await Send.OkAsync(cancellation: ct);
    }
}

public class ResponseGamePauseRequest {
    public Guid GameId { get; set; }
    public required string Username { get; set; }
    public required bool Approved { get; set; }
}
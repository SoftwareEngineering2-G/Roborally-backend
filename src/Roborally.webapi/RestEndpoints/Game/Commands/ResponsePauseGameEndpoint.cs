using FastEndpoints;
using Roborally.core.application.CommandContracts.Game;

namespace Roborally.webapi.RestEndpoints.Game.Commands;

public class ResponsePauseGameEndpoint : Endpoint<ResponseGamePauseRequest> {
    public override void Configure() {
        Post("/games/{gameId}/pause/respond");
    }

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
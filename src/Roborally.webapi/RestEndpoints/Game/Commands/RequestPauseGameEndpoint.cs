using FastEndpoints;
using Roborally.core.application.CommandContracts.Game;

namespace Roborally.webapi.RestEndpoints.Game.Commands;

public class RequestPauseGameEndpoint : Endpoint<RequestGamePauseRequest> {
    public override void Configure() {
        Post("/games/{gameId}/pause/request");
    }

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
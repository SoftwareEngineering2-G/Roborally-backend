using FastEndpoints;
using Roborally.core.application.CommandContracts.Game;

namespace Roborally.webapi.RestEndpoints.Game.Commands;

public class ActivateNextBoardElementEndpoint : Endpoint<ActivateNextBoardElementRequest> {
/// <author name="nilanjanadevkota 2025-10-14 19:37:00 +0200 7" />
    public override void Configure() {
        Post("/games/{gameId}/activate-next-board-element");
    }

/// <author name="nilanjanadevkota 2025-10-14 19:37:00 +0200 11" />
    public override async Task HandleAsync(ActivateNextBoardElementRequest req, CancellationToken ct) {
        ActivateNextBoardElementCommand command = new ActivateNextBoardElementCommand {
            GameId = req.GameId
        };

        await command.ExecuteAsync(ct);
        await Send.OkAsync(cancellation: ct);
    }
}

public class ActivateNextBoardElementRequest {
    public Guid GameId { get; set; }
}
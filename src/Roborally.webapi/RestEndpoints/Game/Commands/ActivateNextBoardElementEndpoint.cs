using FastEndpoints;
using Roborally.core.application.CommandContracts.Game;

namespace Roborally.webapi.RestEndpoints.Game.Commands;

public class ActivateNextBoardElementEndpoint : Endpoint<ActivateNextBoardElementRequest> {
    public override void Configure() {
        Post("/games/{gameId}/activate-next-board-element");
    }

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
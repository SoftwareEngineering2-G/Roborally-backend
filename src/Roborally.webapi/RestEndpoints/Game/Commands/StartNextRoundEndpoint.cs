using FastEndpoints;
using Roborally.core.application.CommandContracts.Game;

namespace Roborally.webapi.RestEndpoints.Game.Commands;

public class StartNextRoundEndpoint : Endpoint<StartNextRoundRequest> {
    public override void Configure() {
        Post("/games/{gameId}/start-next-round");
    }

    public override async Task HandleAsync(StartNextRoundRequest req, CancellationToken ct) {
        StartNextRoundCommand command = new StartNextRoundCommand {
            GameId = req.GameId
        };

        await command.ExecuteAsync(ct);
        await Send.OkAsync(cancellation: ct);
    }
}

public class StartNextRoundRequest {
    public Guid GameId { get; set; }
}


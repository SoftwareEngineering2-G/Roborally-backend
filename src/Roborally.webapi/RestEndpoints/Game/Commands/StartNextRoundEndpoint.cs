using FastEndpoints;
using Roborally.core.application.CommandContracts.Game;

namespace Roborally.webapi.RestEndpoints.Game.Commands;

public class StartNextRoundEndpoint : Endpoint<StartNextRoundRequest> {
/// <author name="Suhani Pandey 2025-12-03 21:46:28 +0100 7" />
    public override void Configure() {
        Post("/games/{gameId}/start-next-round");
    }

/// <author name="Suhani Pandey 2025-12-03 21:46:28 +0100 11" />
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

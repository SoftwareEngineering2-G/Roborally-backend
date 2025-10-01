using FastEndpoints;
using Roborally.core.application.CommandContracts.Game;

namespace Roborally.webapi.RestEndpoints.Game;

public class DealDecksToAllEndpoint : Endpoint<DealDecksToAllRequest> {
    public override void Configure() {
        Post("/games/{gameId}/deal-decks-to-all");
    }

    public override async Task HandleAsync(DealDecksToAllRequest req, CancellationToken ct) {
        DealDecksToAllCommand command = new DealDecksToAllCommand() {
            GameId = req.GameId,
            HostUsername = req.Username
        };

        await command.ExecuteAsync(ct);
        await Send.OkAsync(cancellation: ct);
    }
}

public class DealDecksToAllRequest {
    public string Username { get; set; }
    public Guid GameId { get; set; }
}
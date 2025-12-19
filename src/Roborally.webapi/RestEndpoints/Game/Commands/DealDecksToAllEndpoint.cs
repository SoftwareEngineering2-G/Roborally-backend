using FastEndpoints;
using Roborally.core.application.CommandContracts.Game;

namespace Roborally.webapi.RestEndpoints.Game.Commands;

public class DealDecksToAllEndpoint : Endpoint<DealDecksToAllRequest> {
/// <author name="Sachin Baral 2025-10-01 21:53:45 +0200 7" />
    public override void Configure() {
        Post("/games/{gameId}/deal-decks-to-all");
    }

/// <author name="Sachin Baral 2025-10-01 21:53:45 +0200 11" />
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
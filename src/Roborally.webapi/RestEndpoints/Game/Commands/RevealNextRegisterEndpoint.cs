using FastEndpoints;
using Roborally.core.application.CommandContracts.Game;

namespace Roborally.webapi.RestEndpoints.Game.Commands;

public class RevealNextRegisterEndpoint : Endpoint<RevealNextRegisterRequest> {
/// <author name="Sachin Baral 2025-11-04 21:24:56 +0100 7" />
    public override void Configure() {
        Post("/games/{gameId}/reveal-next-register");
    }

/// <author name="Sachin Baral 2025-11-04 21:24:56 +0100 11" />
    public override async Task HandleAsync(RevealNextRegisterRequest req, CancellationToken ct) {
        RevealNextRegisterCommand command = new RevealNextRegisterCommand() {
            GameId = req.GameId,
            Username = req.Username
        };

        await command.ExecuteAsync(ct);
        await Send.OkAsync(cancellation: ct);
    }
}

public class RevealNextRegisterRequest {
    public required string Username { get; init; }
    public Guid GameId { get; init; }
}
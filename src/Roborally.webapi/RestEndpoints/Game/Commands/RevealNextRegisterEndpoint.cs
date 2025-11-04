using FastEndpoints;
using Roborally.core.application.CommandContracts.Game;

namespace Roborally.webapi.RestEndpoints.Game.Commands;

public class RevealNextRegisterEndpoint : Endpoint<RevealNextRegisterRequest> {
    public override void Configure() {
        Post("/games/{gameId}/reveal-next-register");
    }

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
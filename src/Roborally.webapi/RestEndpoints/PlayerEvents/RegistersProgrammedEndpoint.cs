using FastEndpoints;
using Roborally.core.application.CommandContracts.PlayerEvents;

namespace Roborally.webapi.RestEndpoints.PlayerEvents;

public class RegistersProgrammedEndpoint : Endpoint<RegisterProgrammedRequest> {
    public override void Configure() {
        Post("/games/{gameId}/registers-programmed");
    }


    public override async Task HandleAsync(RegisterProgrammedRequest req, CancellationToken ct) {
        RegistersProgrammedCommand command = new RegistersProgrammedCommand() {
            GameId = req.GameId,
            Username = req.Username,
            LockedInCardsInOrder = req.LockedCardsInOrder
        };

        await command.ExecuteAsync(ct);
        await Send.OkAsync(cancellation: ct);
    }
}

public class RegisterProgrammedRequest {
    public string Username { get; set; }
    public Guid GameId { get; set; }
    public List<string> LockedCardsInOrder { get; set; } = [];
}
using FastEndpoints;
using Roborally.core.application.CommandContracts.Game;

namespace Roborally.webapi.RestEndpoints.Game.Commands;

public class StartActivationPhaseEndpoint : Endpoint<StartActivationPhaseRequest>
{
/// <author name="Suhani Pandey 2025-10-10 13:01:53 +0200 8" />
    public override void Configure()
    {
        Post("/games/{gameId}/start-activation-phase");
    }

/// <author name="Suhani Pandey 2025-10-10 13:01:53 +0200 13" />
    public override async Task HandleAsync(StartActivationPhaseRequest req, CancellationToken ct)
    {
        StartActivationPhaseCommand command = new StartActivationPhaseCommand
        {
            GameId = req.GameId,
            Username = req.Username
        };

        await command.ExecuteAsync(ct);
        await Send.OkAsync(cancellation: ct);
    }

}

public class StartActivationPhaseRequest
{
    public string Username { get; set; }
    public Guid GameId { get; set; }
}
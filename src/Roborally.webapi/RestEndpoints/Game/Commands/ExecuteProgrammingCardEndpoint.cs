using FastEndpoints;
using Roborally.core.application.CommandContracts.Game;

namespace Roborally.webapi.RestEndpoints.Game.Commands;

public class ExecuteProgrammingCardEndpoint : Endpoint<ExecuteProgrammingCardRequest>
{
    public override void Configure()
    {
        Post("/games/{gameId}/players/{username}/execute-card");
    }

    public override async Task HandleAsync(ExecuteProgrammingCardRequest req, CancellationToken ct)
    {
        var command = new ExecuteProgrammingCardCommand
        {
            GameId = req.GameId,
            Username = req.Username,
            CardName = req.CardName
        };

        await command.ExecuteAsync(ct);

        await Send.OkAsync(cancellation: ct);
    }
}

public class ExecuteProgrammingCardRequest
{
    public Guid GameId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string CardName { get; set; } = string.Empty;
}

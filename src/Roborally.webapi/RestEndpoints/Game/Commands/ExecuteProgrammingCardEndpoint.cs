using FastEndpoints;
using Roborally.core.application.CommandContracts.Game;

namespace Roborally.webapi.RestEndpoints.Game.Commands;

public class ExecuteProgrammingCardEndpoint : Endpoint<ExecuteProgrammingCardRequest, ExecuteProgrammingCardResponse>
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

        var response = await command.ExecuteAsync(ct);

        await Send.OkAsync(new ExecuteProgrammingCardResponse
        {
            PositionX = response.PlayerState.PositionX,
            PositionY = response.PlayerState.PositionY,
            Direction = response.PlayerState.Direction
        }, ct);
    }
}

public class ExecuteProgrammingCardRequest
{
    public Guid GameId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string CardName { get; set; } = string.Empty;
}

public class ExecuteProgrammingCardResponse
{
    public required int PositionX { get; set; }
    public required int PositionY { get; set; }
    public required string Direction { get; set; }
}

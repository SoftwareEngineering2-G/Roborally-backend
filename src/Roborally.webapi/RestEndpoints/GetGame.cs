using FastEndpoints;
using Roborally.core.application.CommandContracts;
using Roborally.core.domain.Game;

namespace Roborally.webapi.RestEndpoints;

public class GetGame:Endpoint<GetGameRequest,GetGameResponse>
{
    public override void Configure()
    {
        Get("/game/{gameId}");
    }

    public override async Task HandleAsync(GetGameRequest req, CancellationToken ct)
    {
        var command = new GetGameCommand()
        {
            GameId = req.GameId
        };
        Game game = await command.ExecuteAsync(ct);
        await Send.OkAsync(new GetGameResponse()
        {
            game = game
        },ct);
    }
}
public class GetGameRequest
{
    public Guid GameId { get; set; }
}
public class GetGameResponse
{
    public Game game { get; set; }
}
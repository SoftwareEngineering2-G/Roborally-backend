using FastEndpoints;
using Roborally.core.application.CommandContracts;

namespace Roborally.webapi.RestEndpoints.Game;

public class GetGameEndpoint:Endpoint<GetGameRequest,GetGameResponse>
{
    public override void Configure()
    {
        Get("/game/{GameId}");
    }

    public override async Task HandleAsync(GetGameRequest req, CancellationToken ct)
    {
        GetGameCommand command = new GetGameCommand()
        {
            GameId = req.GameId
        };
        GetGameCommandResponse game = await command.ExecuteAsync(ct);
        
        await Send.OkAsync(new GetGameResponse()
        {
            Game = game.Game
        }, ct);
        
    }
 
}

public class GetGameRequest
{
    public Guid GameId { get; set; }
}
public class GetGameResponse
{
    public core.domain.Game.Game Game { get; set; }
}
using FastEndpoints;
using Roborally.core.application.CommandContracts;
using Roborally.core.domain.Game;

namespace Roborally.webapi.RestEndpoints;

public class GetGame:EndpointWithoutRequest<GetGameResponse>
{
    public override void Configure()
    {
        Get("/game");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        GetGameCommand command = new GetGameCommand();
        Game game = await command.ExecuteAsync(ct);
        await Send.OkAsync(new GetGameResponse
        {
            game = game
        }, ct);
    }
}

public class GetGameResponse
{
    public Game game { get; set; }
}
using FastEndpoints;

namespace Roborally.webapi.RestEndpoints;

public class GetGamesByPlayer : Endpoint<GamesByPlayerRequest, GetGamesByPlayerResponse>
{
    public override void Configure()
    {
        Get("/games-by-player");
    }
    public override async Task HandleAsync(GamesByPlayerRequest req, CancellationToken ct)
    {
        core.application.CommandContracts.GetGamesByPlayerCommand command = new core.application.CommandContracts.GetGamesByPlayerCommand()
        {
            PlayerId = req.playerId
        };
        List<core.domain.Game.Game> games = await command.ExecuteAsync(ct);
        await Send.OkAsync(new GetGamesByPlayerResponse
        {
            games = games
        }, ct);
    }

   
}

public class GetGamesByPlayerResponse
{
    public List<core.domain.Game.Game> games { get; set; }
}

public class GamesByPlayerRequest
{
    public Guid playerId { get; set; }
}

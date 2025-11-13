using FastEndpoints;
using Roborally.core.application.CommandContracts;

namespace Roborally.webapi.RestEndpoints.Lobby;

public class EndGameEndpoint: Endpoint<EndGameRequest>
{
    public override void Configure()
    {
        Post("/games/{gameId}/end");
    }
    
    public override async Task HandleAsync(EndGameRequest req, CancellationToken ct)
    {
        EndGameCommand command = new EndGameCommand()
        {
            GameId = req.GameId
        };
        
        await command.ExecuteAsync(ct);

        await Send.OkAsync(cancellation: ct);
    }
    
}

public class EndGameRequest
{
    public Guid GameId { get; set; }
}
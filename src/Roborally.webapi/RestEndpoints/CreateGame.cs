using FastEndpoints;

namespace Roborally.webapi.RestEndpoints;

public class CreateGame : Endpoint<CreateGameRequest, CreateGameResponse>
{
    public override void Configure()
    {
        Post("/game");
    }
    
    public override async Task HandleAsync(CreateGameRequest req, CancellationToken ct)
    {
        core.application.CommandContracts.CreateGameCommand command = new core.application.CommandContracts.CreateGameCommand()
        {
            PlayerIds = req.PlayerIds,
            GameBoardId = req.GameBoardId
        };
        var id =await command.ExecuteAsync(ct);
        await Send.OkAsync(new CreateGameResponse()
        {
            GameId = Guid.Parse(id)
        },ct);
    }
}

public class CreateGameRequest
{
    public IList<Guid> PlayerIds { get; set; }
    public Guid GameBoardId { get; set; }
}

public class CreateGameResponse
{
    public Guid GameId { get; set; }   
}
using FastEndpoints;
using Roborally.core.application.CommandContracts;

namespace Roborally.webapi.RestEndpoints;

public class CreateGameBoard:Endpoint<AddGameBoardRequest,AddGameBoardResponse>
{
    public override void Configure()
    {
        Post("/game-board");
    }

    public override async Task HandleAsync(AddGameBoardRequest req, CancellationToken ct)
    {
        CreateGameBoardCommand command = new CreateGameBoardCommand()
        {
            BoardSpaces = req.BoardSpaces,
        };
        Guid boardId = await command.ExecuteAsync(ct);
        await Send.OkAsync(new AddGameBoardResponse
        {
            BoardId = boardId
        }, ct);
    }

}

public class AddGameBoardRequest
{
    public required string[][] BoardSpaces { get; set; }
}

public class AddGameBoardResponse
{
    public Guid BoardId { get; set; }
}
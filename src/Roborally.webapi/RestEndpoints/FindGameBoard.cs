using FastEndpoints;
using Roborally.core.domain.Game;

namespace Roborally.webapi.RestEndpoints;

public class FindGameBoard:Endpoint<FindGameBoardRequest,FindGameBoardResponse>
{
    public override void Configure()
    {
        Get("/game-board/{BoardId}");
    }
    
    public override async Task HandleAsync(FindGameBoardRequest req, CancellationToken ct)
    {
        var command = new Roborally.core.application.CommandContracts.FindGameBoardCommand()
        {
            Id = req.BoardId
        };
       GameBoard board= await command.ExecuteAsync(ct);
       await Send.OkAsync(new FindGameBoardResponse()
       {
           BoardSpaces = board.SpaceNames
       },ct);
    }
}

public class FindGameBoardRequest
{
    public required Guid BoardId { get; set; }
}

public class FindGameBoardResponse
{
    public required string[][] BoardSpaces { get; set; }
}
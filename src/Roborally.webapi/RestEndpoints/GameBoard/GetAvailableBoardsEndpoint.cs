using FastEndpoints;
using Roborally.core.application.ApplicationContracts.Persistence;

namespace Roborally.webapi.RestEndpoints.GameBoard;

public class GetAvailableBoardsEndpoint : EndpointWithoutRequest<List<BoardInfo>>
{
    private readonly IGameBoardRepository _gameBoardRepository;

    public GetAvailableBoardsEndpoint(IGameBoardRepository gameBoardRepository)
    {
        _gameBoardRepository = gameBoardRepository;
    }

    public override void Configure()
    {
        Get("/game-boards");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var boards = await _gameBoardRepository.GetAllAsync(ct);
        
        var boardInfos = boards.Select(b => new BoardInfo
        {
            Name = b.Name
        }).ToList();

        await Send.OkAsync(boardInfos, ct);
    }
}

public class BoardInfo
{
    public string Name { get; set; } = string.Empty;
}

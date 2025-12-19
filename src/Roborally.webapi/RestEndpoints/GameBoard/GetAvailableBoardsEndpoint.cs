using FastEndpoints;
using Roborally.core.application.ApplicationContracts.Persistence;

namespace Roborally.webapi.RestEndpoints.GameBoard;

public class GetAvailableBoardsEndpoint : EndpointWithoutRequest<List<BoardInfo>>
{
    private readonly IGameBoardRepository _gameBoardRepository;

/// <author name="Satish 2025-11-03 14:12:46 +0100 10" />
    public GetAvailableBoardsEndpoint(IGameBoardRepository gameBoardRepository)
    {
        _gameBoardRepository = gameBoardRepository;
    }

/// <author name="Satish 2025-11-03 14:12:46 +0100 15" />
    public override void Configure()
    {
        Get("/game-boards");
        AllowAnonymous();
    }

/// <author name="Satish 2025-11-03 14:12:46 +0100 21" />
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
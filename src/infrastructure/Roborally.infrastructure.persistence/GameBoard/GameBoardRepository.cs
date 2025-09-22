using Roborally.core.domain.Game;

namespace Roborally.infrastructure.persistence.GameBoard;

public class GameBoardRepository:IGameBoardRepository
{
    
    private readonly AppDatabaseContext _context;
    
    public GameBoardRepository(AppDatabaseContext context)
    {
        _context = context;
    }
    public async Task AddAsync(core.domain.Game.GameBoard gameBoard, CancellationToken cancellationToken = default)
    {
        await _context.GameBoards.AddAsync(gameBoard, cancellationToken);
    }

    public async Task<core.domain.Game.GameBoard?> FindAsync(Guid gameBoardId, CancellationToken cancellationToken = default)
    {
        return await _context.GameBoards.FindAsync(new object?[] { gameBoardId }, cancellationToken);
    }
}
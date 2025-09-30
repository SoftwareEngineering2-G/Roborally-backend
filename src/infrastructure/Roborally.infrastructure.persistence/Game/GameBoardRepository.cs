using Roborally.core.domain.Game.Gameboard;

namespace Roborally.infrastructure.persistence.Game;

public class GameBoardRepository : IGameBoardRepository {
    private readonly AppDatabaseContext _context;

    public GameBoardRepository(AppDatabaseContext context) {
        _context = context;
    }

    public Task<GameBoard?> FindAsync(string name, CancellationToken ct = default) {
        return _context.GameBoards.FindAsync([name], ct).AsTask();
    }

    public Task AddAsync(GameBoard board, CancellationToken ct = default) {
        return _context.GameBoards.AddAsync(board, ct).AsTask();
    }
}
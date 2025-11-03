using Microsoft.EntityFrameworkCore;
using Roborally.core.application;
using Roborally.core.application.ApplicationContracts.Persistence;
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

    public async Task<List<GameBoard>> GetAllAsync(CancellationToken ct = default) {
        return await _context.GameBoards.ToListAsync(ct);
    }

    public Task AddAsync(GameBoard board, CancellationToken ct = default) {
        return _context.GameBoards.AddAsync(board, ct).AsTask();
    }
}
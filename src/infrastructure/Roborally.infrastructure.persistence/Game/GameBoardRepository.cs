using Microsoft.EntityFrameworkCore;
using Roborally.core.application;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.domain.Game.Gameboard;

namespace Roborally.infrastructure.persistence.Game;

public class GameBoardRepository : IGameBoardRepository {
    private readonly AppDatabaseContext _context;

/// <author name="Sachin Baral 2025-10-01 21:53:45 +0200 11" />
    public GameBoardRepository(AppDatabaseContext context) {
        _context = context;
    }

/// <author name="Sachin Baral 2025-10-01 21:53:45 +0200 15" />
    public Task<GameBoard?> FindAsync(string name, CancellationToken ct = default) {
        return _context.GameBoards.FindAsync([name], ct).AsTask();
    }

/// <author name="Satish Gurung 2025-11-03 14:12:46 +0100 19" />
    public async Task<List<GameBoard>> GetAllAsync(CancellationToken ct = default) {
        return await _context.GameBoards.ToListAsync(ct);
    }

/// <author name="Sachin Baral 2025-10-01 21:53:45 +0200 23" />
    public Task AddAsync(GameBoard board, CancellationToken ct = default) {
        return _context.GameBoards.AddAsync(board, ct).AsTask();
    }
}
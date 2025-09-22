using Microsoft.EntityFrameworkCore;
using Roborally.core.domain.Game;

namespace Roborally.infrastructure.persistence.Game;

public class GameRepository:IGameRepository
{
    private readonly AppDatabaseContext _context;
    
    public GameRepository(AppDatabaseContext context)
    {
        _context = context;
    }
    public async Task AddAsync(core.domain.Game.Game game, CancellationToken cancellationToken = default)
    {
        await _context.Games.AddAsync(game, cancellationToken);
    }

    public async Task<core.domain.Game.Game?> FindAsync(Guid gameId, CancellationToken cancellationToken = default)
    {
        return await _context.Games.Include(x=>x.Players)
            .Include(x=>x.GameBoard)
            .FirstOrDefaultAsync(g => g.Id == gameId, cancellationToken);
    }

    public async Task<List<core.domain.Game.Game>> ListAllByPlayerIdAsync(Guid playerId, CancellationToken cancellationToken = default)
    {
        return await _context.Games.Include(x=>x.Players)
            .Include(x=>x.GameBoard)
            .Where(g => g.Players.Any(p => p.Id == playerId))
            .ToListAsync(cancellationToken);
    }
}
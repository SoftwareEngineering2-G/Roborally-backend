using Microsoft.EntityFrameworkCore;
using Roborally.core.domain.Game;

namespace Roborally.infrastructure.persistence.Game;

public class GameRepository : IGameRepository{

    private readonly AppDatabaseContext _context;

    public GameRepository(AppDatabaseContext context) {
        _context = context;
    }

    public Task AddAsync(core.domain.Game.Game game, CancellationToken ct) {
        return _context.Games.AddAsync(game, ct).AsTask();
    }

    public Task<core.domain.Game.Game?> FindAsync(Guid gameId, CancellationToken ct) {
        return _context.Games.Include(game => game.Players)
            .ThenInclude(player => player.PlayerEvents)
            .Include(game=> game.GameBoard)
            .FirstOrDefaultAsync(game => game.GameId.Equals(gameId), ct);
    }
}
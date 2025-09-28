using Microsoft.EntityFrameworkCore;
using Roborally.core.domain.Game.Player;

namespace Roborally.infrastructure.persistence.Player;

public class PlayerRepository : IPlayerRepository {
    private readonly AppDatabaseContext _context;

    public PlayerRepository(AppDatabaseContext context) {
        _context = context;
    }

    public Task<core.domain.Game.Player.Player?> FindAsync(string username, Guid gameId, CancellationToken ct) {
        return _context.Players.Include(player => player.PlayerEvents).FirstOrDefaultAsync(
            player => player.Username.ToLower().Equals(username.ToLower()) && player.GameId.Equals(gameId), ct);
    }

    public Task AddAsync(core.domain.Game.Player.Player player, CancellationToken ct) {
        return _context.Players.AddAsync(player, ct).AsTask();
    }
}
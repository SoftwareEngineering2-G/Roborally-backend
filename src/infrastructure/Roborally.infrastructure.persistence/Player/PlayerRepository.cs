using Microsoft.EntityFrameworkCore;
using Roborally.core.domain.Game;

namespace Roborally.infrastructure.persistence.Player;

public class PlayerRepository:IPlayerRepository
{
    private readonly AppDatabaseContext _context;
    
    public PlayerRepository(AppDatabaseContext context)
    {
        _context = context;
    }
    public async Task AddAsync(core.domain.Game.Player player, CancellationToken cancellationToken = default)
    {
        await _context.Players.AddAsync(player, cancellationToken);
    }

    public async Task<core.domain.Game.Player?> FindAsync(Guid playerId, CancellationToken cancellationToken = default)
    {
        return await _context.Players.FirstOrDefaultAsync(p => p.Id == playerId, cancellationToken);
    }
}
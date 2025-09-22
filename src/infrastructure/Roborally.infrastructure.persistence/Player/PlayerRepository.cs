using Roborally.core.domain.Game;

namespace Roborally.infrastructure.persistence.Player;

public class PlayerRepository:IPlayerRepository
{
    public async Task AddAsync(core.domain.Game.Player player, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<core.domain.Game.Player> FindAsync(Guid playerId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
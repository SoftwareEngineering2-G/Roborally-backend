using Roborally.core.domain.Game;

namespace Roborally.infrastructure.persistence.Game;

public class GameRepository:IGameRepository
{
    public async Task AddAsync(core.domain.Game.Game game, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<core.domain.Game.Game?> FindAsync(Guid gameId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
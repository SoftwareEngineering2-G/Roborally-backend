namespace Roborally.core.domain.Game;

public interface IGameRepository
{
    Task AddAsync(Game game, CancellationToken cancellationToken = default);
    Task<Game?> FindAsync(Guid gameId, CancellationToken cancellationToken = default);
    Task<List<Game>> ListAllByPlayerIdAsync(Guid playerId, CancellationToken cancellationToken = default);
}
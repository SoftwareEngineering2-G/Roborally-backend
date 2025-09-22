namespace Roborally.core.domain.Game;

public interface IPlayerRepository
{
    Task AddAsync(Player player, CancellationToken cancellationToken = default);
    Task<Player?> FindAsync(Guid playerId, CancellationToken cancellationToken = default);
}
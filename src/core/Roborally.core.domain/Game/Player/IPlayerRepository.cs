namespace Roborally.core.domain.Game.Player;

public interface IPlayerRepository {

    Task<Player?> FindAsync(string username, Guid gameId, CancellationToken ct);
    Task AddAsync(Player player, CancellationToken ct);
    
}
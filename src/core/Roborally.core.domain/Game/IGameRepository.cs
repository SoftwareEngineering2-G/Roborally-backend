namespace Roborally.core.domain.Game;

public interface IGameRepository {

    Task AddAsync(Game game, CancellationToken ct);
    Task<Game?> FindAsync(Guid gameId, CancellationToken ct);
}

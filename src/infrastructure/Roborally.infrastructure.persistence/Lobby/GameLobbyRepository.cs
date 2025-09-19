using Microsoft.EntityFrameworkCore;
using Roborally.core.domain.Lobby;

namespace Roborally.infrastructure.persistence.Lobby;

public class GameLobbyRepository : IGameLobbyRepository {
    private readonly AppDatabaseContext _context;

    public GameLobbyRepository(AppDatabaseContext context) {
        _context = context;
    }

    public Task AddAsync(GameLobby gameLobby, CancellationToken cancellationToken = default) {
        return _context.GameLobby.AddAsync(gameLobby, cancellationToken).AsTask();
    }

    public async Task<bool> IsUserCurrentlyHostingActiveLobbyAsync(string hostUsername) {
        return await _context.GameLobby
            .AnyAsync(x => x.HostUsername.ToLower().Equals(hostUsername.ToLower()) && x.StartedAt == null);
    }

    public Task<GameLobby?> FindAsync(Guid gameId) {
        return _context.GameLobby.Include(gl => gl.JoinedUsers)
            .FirstOrDefaultAsync(gl => gl.GameId.Equals(gameId));
    }
}
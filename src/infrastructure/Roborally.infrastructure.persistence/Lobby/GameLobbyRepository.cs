using Microsoft.EntityFrameworkCore;
using Roborally.core.application;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.domain.Lobby;

namespace Roborally.infrastructure.persistence.Lobby;

public class GameLobbyRepository : IGameLobbyRepository {
    private readonly AppDatabaseContext _context;

/// <author name="Sachin Baral 2025-09-19 13:01:07 +0200 11" />
    public GameLobbyRepository(AppDatabaseContext context) {
        _context = context;
    }

/// <author name="Sachin Baral 2025-09-19 13:01:07 +0200 15" />
    public Task AddAsync(GameLobby gameLobby, CancellationToken cancellationToken = default) {
        return _context.GameLobby.AddAsync(gameLobby, cancellationToken).AsTask();
    }

/// <author name="Sachin Baral 2025-09-19 13:01:07 +0200 19" />
    public async Task<bool> IsUserCurrentlyHostingActiveLobbyAsync(string hostUsername) {
        return await _context.GameLobby
            .AsNoTracking()
            .AnyAsync(x => x.HostUsername.ToLower().Equals(hostUsername.ToLower()) && x.StartedAt == null);
    }

/// <author name="Sachin Baral 2025-09-19 13:01:07 +0200 25" />
    public Task<GameLobby?> FindAsync(Guid gameId) {
        return _context.GameLobby
            .Include(gl => gl.JoinedUsers)
            .Include(gl => gl.RequiredUsers)
            .FirstOrDefaultAsync(gl => gl.GameId.Equals(gameId));
    }

/// <author name="Sachin Baral 2025-09-21 21:45:34 +0200 32" />
    public Task<List<GameLobby>> FindPublicLobbiesAsync(CancellationToken cancellationToken = default)
    {
        return _context.GameLobby
            .Where(x => !x.IsPrivate && x.StartedAt == null &&
                        x.JoinedUsers.Count < 6) // TODO: change 6 to a variable if max lobby size becomes configurable
            .Include(x => x.JoinedUsers)
            .ToListAsync(cancellationToken);
    }


/// <author name="Vincenzo Altaserse 2025-10-18 13:11:59 +0200 42" />
    public void Remove(GameLobby gameLobby) {
        _context.GameLobby.Remove(gameLobby);
    }
}
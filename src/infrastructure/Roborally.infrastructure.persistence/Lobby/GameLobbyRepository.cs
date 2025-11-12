using Microsoft.EntityFrameworkCore;
using Roborally.core.application;
using Roborally.core.application.ApplicationContracts.Persistence;
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
            .AsNoTracking()
            .AnyAsync(x => x.HostUsername.ToLower().Equals(hostUsername.ToLower()) && x.StartedAt == null);
    }

    public Task<GameLobby?> FindAsync(Guid gameId) {
        return _context.GameLobby
            .Include(gl => gl.JoinedUsers)
            .Include(gl => gl.RequiredUsers)
            .FirstOrDefaultAsync(gl => gl.GameId.Equals(gameId));
    }

    public Task<List<GameLobby>> FindPublicLobbiesAsync(CancellationToken cancellationToken = default)
    {
        return _context.GameLobby
            .Where(x => !x.IsPrivate && x.StartedAt == null &&
                        x.JoinedUsers.Count < 6) // TODO: change 6 to a variable if max lobby size becomes configurable
            .Include(x => x.JoinedUsers)
            .ToListAsync(cancellationToken);
    }


    public void Remove(GameLobby gameLobby) {
        _context.GameLobby.Remove(gameLobby);
    }
}
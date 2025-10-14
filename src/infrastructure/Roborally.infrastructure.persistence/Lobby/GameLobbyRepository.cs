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

    public Task<List<GameLobby>> FindPublicLobbiesAsync(CancellationToken cancellationToken = default)
    {
        return _context.GameLobby
            .Where(x => !x.IsPrivate && x.StartedAt == null &&
                        x.JoinedUsers.Count < 6) // TODO: change 6 to a variable if max lobby size becomes configurable
            .Include(x => x.JoinedUsers)
            .ToListAsync(cancellationToken);
    }

    public async Task<GameLobby?> GetLobbyByIdAsync(Guid gameLobbyId, CancellationToken cancellationToken = default)
    {
        return await _context.GameLobby
            .Include(x => x.JoinedUsers)
            .FirstOrDefaultAsync(x => x.GameId == gameLobbyId, cancellationToken);
    }

    public Task JoinLobbyAsync(GameLobby lobby, core.domain.User.User user,
        CancellationToken cancellationToken = default)
    {
        lobby.JoinLobby(user);
        _context.GameLobby.Update(lobby);
        return Task.CompletedTask;
    }


}
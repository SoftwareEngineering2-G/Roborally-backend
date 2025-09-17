using Microsoft.EntityFrameworkCore;
using Roborally.core.domain.Lobby;

namespace Roborally.infrastructure.persistence.Lobby;

public class GameLobbyRepository : IGameLobbyRepository
{
    private readonly AppDatabaseContext _context;

    public GameLobbyRepository(AppDatabaseContext context)
    {
        _context = context;
    }

    public Task AddAsync(GameLobby gameLobby, CancellationToken cancellationToken = default)
    {
        return _context.GameLobby.AddAsync(gameLobby, cancellationToken).AsTask();
    }

    public async Task<bool> IsUserCurrentlyHostingActiveLobbyAsync(Guid hostUserId)
    {
        return await _context.GameLobby
            .AnyAsync(x => x.HostId == hostUserId && x.StartedAt == null);
    }
}
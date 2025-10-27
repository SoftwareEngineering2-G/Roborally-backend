using Roborally.core.domain.Lobby;

namespace Roborally.core.application.ApplicationContracts.Persistence;

public interface IGameLobbyRepository
{
    Task AddAsync(GameLobby gameLobby, CancellationToken cancellationToken = default);
    Task<bool> IsUserCurrentlyHostingActiveLobbyAsync(string hostUsername);
    
    Task<GameLobby?> FindAsync(Guid gameId);
    Task<List<GameLobby>> FindPublicLobbiesAsync(CancellationToken cancellationToken = default);
    
    void Remove(GameLobby gameLobby);
}
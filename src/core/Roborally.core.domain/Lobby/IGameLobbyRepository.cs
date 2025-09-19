namespace Roborally.core.domain.Lobby;

public interface IGameLobbyRepository
{
    Task AddAsync(GameLobby gameLobby, CancellationToken cancellationToken = default);
    Task<bool> IsUserCurrentlyHostingActiveLobbyAsync(string hostUsername);
    Task<GameLobby?> FindAsync(Guid gameId);
}
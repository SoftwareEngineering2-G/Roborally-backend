namespace Roborally.core.domain.Lobby;

public interface IGameLobbyRepository
{
    Task AddAsync(GameLobby gameLobby, CancellationToken cancellationToken = default);
    Task<GameLobby?> FindByHostIdAsync(Guid hostUserId);
}
namespace Roborally.core.application.ApplicationContracts.Broadcasters;

public interface IGameLobbyBroadcaster
{
    Task BroadcastUserJoinedAsync(Guid gameId, string username, CancellationToken cancellationToken = default);
    Task BroadcastUserLeftAsync(Guid gameId, string username, CancellationToken cancellationToken = default);
    Task BroadcastHostChangedAsync(Guid gameId, string newHost, CancellationToken cancellationToken = default);
        
    Task BroadcastGameStartedAsync(Guid gameId, CancellationToken cancellationToken = default);
    Task BroadcastGameContinuedAsync(Guid gameId, CancellationToken cancellationToken = default);
}

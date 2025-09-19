namespace Roborally.core.application;

public interface IGameLobbyBroadcaster
{
    Task BroadcastUserJoinedAsync(Guid gameId, string username, CancellationToken cancellationToken = default);
    Task BroadcastUserLeftAsync(Guid gameId, string username, CancellationToken cancellationToken = default);
}

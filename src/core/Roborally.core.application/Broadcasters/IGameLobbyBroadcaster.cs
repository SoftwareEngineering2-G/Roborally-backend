namespace Roborally.core.application.Broadcasters;

public interface IGameLobbyBroadcaster
{
    Task BroadcastUserJoinedAsync(Guid gameId, string username, CancellationToken cancellationToken = default);
    Task BroadcastUserLeftAsync(Guid gameId, string username, CancellationToken cancellationToken = default);

    Task BroadcastGameStartedAsync(Guid gameId, CancellationToken cancellationToken = default);
}

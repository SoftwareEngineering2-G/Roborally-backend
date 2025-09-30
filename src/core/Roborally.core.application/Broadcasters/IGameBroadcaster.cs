namespace Roborally.core.application.Broadcasters;

public interface IGameBroadcaster {
    Task BroadcastPlayerLockedInRegisterAsync(string username, Guid gameId, CancellationToken ct);

}
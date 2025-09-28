namespace Roborally.core.application.Broadcasters;

public interface IPlayerEventsBroadcaster {
    Task BroadcastPlayerLockedInRegisterAsync(string username, Guid gameId, CancellationToken ct);

}
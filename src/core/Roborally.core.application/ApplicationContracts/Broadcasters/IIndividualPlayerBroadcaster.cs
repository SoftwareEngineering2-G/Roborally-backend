namespace Roborally.core.application.ApplicationContracts.Broadcasters;

public interface IIndividualPlayerBroadcaster {
    public Task BroadcastHandToPlayerAsync(string username, Guid gameId, List<string> dealtCards, CancellationToken ct);
}
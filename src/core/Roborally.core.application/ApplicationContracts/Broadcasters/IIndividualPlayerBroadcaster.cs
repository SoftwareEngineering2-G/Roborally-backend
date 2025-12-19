namespace Roborally.core.application.ApplicationContracts.Broadcasters;

public interface IIndividualPlayerBroadcaster {
/// <author name="Truong Son NGO 2025-11-28 15:36:33 +0100 4" />
    public Task BroadcastHandToPlayerAsync(string username, Guid gameId, List<string> dealtCards, bool isDeckReshuffled, int programmingPickPilesCount, int discardPilesCount, CancellationToken ct);
}
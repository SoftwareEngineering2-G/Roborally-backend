using Microsoft.AspNetCore.SignalR;
using Roborally.core.application.ApplicationContracts.Broadcasters;
using Roborally.infrastructure.broadcaster.Game;

namespace Roborally.infrastructure.broadcaster.Broadcasters;

public class IndividualPlayerBroadcaster : IIndividualPlayerBroadcaster {
    private readonly IHubContext<GameHub> _hubContext;

/// <author name="Sachin Baral 2025-10-01 21:53:45 +0200 10" />
    public IndividualPlayerBroadcaster(IHubContext<GameHub> hubContext) {
        _hubContext = hubContext;
    }

    public async Task BroadcastHandToPlayerAsync(string username, Guid gameId, List<string> dealtCards, bool isDeckReshuffled, int programmingPickPilesCount, int discardPilesCount,
        CancellationToken ct) {
        string groupName = GroupName.IndividualPlayer(username, gameId.ToString());

        var payload = new {
            gameId,
            username,
            dealtCards,
            isDeckReshuffled,
            programmingPickPilesCount,
            discardPilesCount
        };

        await _hubContext.Clients.Group(groupName).SendAsync("PlayerCardsDealt", payload, ct);
    }
}
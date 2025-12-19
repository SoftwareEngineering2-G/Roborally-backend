using Microsoft.AspNetCore.SignalR;
using Roborally.core.application.ApplicationContracts.Broadcasters;
using Roborally.infrastructure.broadcaster.Game;

namespace Roborally.infrastructure.broadcaster.Broadcasters;

public class IndividualPlayerBroadcaster : IIndividualPlayerBroadcaster {
    private readonly IHubContext<GameHub> _hubContext;

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

    public Task BroadcastProgrammingTimeoutAsync(string username, Guid gameId, List<string> assignedCards, CancellationToken ct)
    {
        string groupName = GroupName.IndividualPlayer(username, gameId.ToString());
        
        var payload = new
        {
            gameId,
            username,
            assignedCards
        };
        return _hubContext.Clients.Groups(groupName).SendAsync("ProgrammingTimeout", payload, ct);    
    }
}
using Microsoft.AspNetCore.SignalR;

namespace Roborally.infrastructure.broadcaster.Game;

public class GameHub : Hub {
/// <author name="Sachin Baral 2025-10-01 21:53:45 +0200 6" />
    public async Task JoinGame(string username, string gameId) {
        await Groups.AddToGroupAsync(Context.ConnectionId, GroupName.Game(gameId));
        await Groups.AddToGroupAsync(Context.ConnectionId, GroupName.IndividualPlayer(username, gameId));
    }

/// <author name="Sachin Baral 2025-10-01 21:53:45 +0200 11" />
    public async Task LeaveGame(string username, string gameId) {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, GroupName.Game(gameId));
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, GroupName.IndividualPlayer(username, gameId));
    }
}
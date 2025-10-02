using Microsoft.AspNetCore.SignalR;

namespace Roborally.infrastructure.broadcaster.Game;

public class GameHub : Hub {
    public async Task JoinGame(string username, string gameId) {
        await Groups.AddToGroupAsync(Context.ConnectionId, GroupName.Game(gameId));
        await Groups.AddToGroupAsync(Context.ConnectionId, GroupName.IndividualPlayer(username, gameId));
    }

    public async Task LeaveGame(string username, string gameId) {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, GroupName.Game(gameId));
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, GroupName.IndividualPlayer(username, gameId));
    }
}
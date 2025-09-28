using Microsoft.AspNetCore.SignalR;

namespace Roborally.infrastructure.broadcaster.Game;

public class GameHub : Hub{

    public async Task JoinGame(string gameId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, Groupname(gameId));
    }

    public async Task LeaveGame(string gameId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, Groupname(gameId));
    }

    private string Groupname(string gameId) => $"lobby-{gameId}";
    
}
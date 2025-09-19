using Microsoft.AspNetCore.SignalR;

namespace Roborally.infrastructure.broadcaster.GameLobby;

public class GameLobbyHub : Hub
{
    public async Task JoinLobby(string gameId)
    {
        var groupId = $"lobby-{gameId}";
        await Groups.AddToGroupAsync(Context.ConnectionId, groupId);
    }

    public async Task LeaveLobby(string gameId)
    {
        var groupId = $"lobby-{gameId}";
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId);
    }


    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        // Groups are automatically cleaned up by SignalR
        await base.OnDisconnectedAsync(exception);
    }
}
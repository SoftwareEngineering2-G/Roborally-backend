using Microsoft.AspNetCore.SignalR;

namespace Roborally.infrastructure.broadcaster.GameLobby;

public class GameLobbyHub : Hub
{
    public async Task JoinLobby(string gameId)
    {
        var groupId = GroupName.GameLobby(gameId);
        await Groups.AddToGroupAsync(Context.ConnectionId, groupId);
    }

    public async Task LeaveLobby(string gameId)
    {
        var groupId = GroupName.GameLobby(gameId);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId);
    }


    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        // SignalR automatically cleans up groups
        await base.OnDisconnectedAsync(exception);
    }
}
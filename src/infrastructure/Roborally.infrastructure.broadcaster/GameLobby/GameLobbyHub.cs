using Microsoft.AspNetCore.SignalR;

namespace Roborally.infrastructure.broadcaster.GameLobby;

public class GameLobbyHub : Hub
{
/// <author name="Sachin Baral 2025-09-19 13:01:07 +0200 7" />
    public async Task JoinLobby(string gameId)
    {
        var groupId = GroupName.GameLobby(gameId);
        await Groups.AddToGroupAsync(Context.ConnectionId, groupId);
    }

/// <author name="Sachin Baral 2025-09-19 13:01:07 +0200 13" />
    public async Task LeaveLobby(string gameId)
    {
        var groupId = GroupName.GameLobby(gameId);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId);
    }


/// <author name="Sachin Baral 2025-09-19 13:01:07 +0200 20" />
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        // SignalR automatically cleans up groups
        await base.OnDisconnectedAsync(exception);
    }
}
using Microsoft.AspNetCore.SignalR;
using Roborally.core.application;
using Roborally.infrastructure.broadcaster.GameLobby;

namespace Roborally.infrastructure.broadcaster;

public class GameLobbyBroadcaster : IGameLobbyBroadcaster
{
    private readonly IHubContext<GameLobbyHub> _hubContext;

    public GameLobbyBroadcaster(IHubContext<GameLobbyHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task BroadcastUserJoinedAsync(Guid gameId, string username, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("GameLobbyBroadcaster: Broadcasting UserJoinedLobby");
        var groupId = $"lobby-{gameId}";
        var payload = new { GameId = gameId, Username = username };
        await _hubContext.Clients.Group(groupId).SendAsync("UserJoinedLobby", payload, cancellationToken);
    }

    public async Task BroadcastUserLeftAsync(Guid gameId, string username, CancellationToken cancellationToken = default)
    {
        var groupId = $"lobby-{gameId}";
        var payload = new { GameId = gameId, Username = username };
        await _hubContext.Clients.Group(groupId).SendAsync("UserLeftLobby", payload, cancellationToken);
    }

}

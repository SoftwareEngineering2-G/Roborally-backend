using Microsoft.AspNetCore.SignalR;
using Roborally.core.application.ApplicationContracts.Broadcasters;
using Roborally.infrastructure.broadcaster.GameLobby;

namespace Roborally.infrastructure.broadcaster.Broadcasters;

public class GameLobbyBroadcaster : IGameLobbyBroadcaster
{
    private readonly IHubContext<GameLobbyHub> _hubContext;
    public GameLobbyBroadcaster(IHubContext<GameLobbyHub> hubContext)
    {
        _hubContext = hubContext;
    }

    private static string GetGroupId(Guid gameId) => GroupName.GameLobby(gameId.ToString());

    public async Task BroadcastUserJoinedAsync(Guid gameId, string username, CancellationToken cancellationToken = default)
    {
        string groupId = GetGroupId(gameId);
        var payload = new { GameId = gameId, Username = username };
        await _hubContext.Clients.Group(groupId).SendAsync("UserJoinedLobby", payload, cancellationToken);
    }

    public async Task BroadcastUserLeftAsync(Guid gameId, string username, CancellationToken cancellationToken = default)
    {
        string groupId = GetGroupId(gameId);
        var payload = new { GameId = gameId, Username = username };
        await _hubContext.Clients.Group(groupId).SendAsync("UserLeftLobby", payload, cancellationToken);
    }
    
    public async Task BroadcastHostChangedAsync(Guid gameId, string newHost, CancellationToken cancellationToken = default)
    {
        string groupId = GetGroupId(gameId);
        var payload = new { GameId = gameId, NewHost = newHost };
        await _hubContext.Clients.Group(groupId).SendAsync("HostChanged", payload, cancellationToken);
    }

    public Task BroadcastGameStartedAsync(Guid gameId, CancellationToken cancellationToken = default) {
        string groupId = GetGroupId(gameId);
        var payload = new { GameId = gameId };  
        return _hubContext.Clients.Group(groupId).SendAsync("GameStarted", payload, cancellationToken);
    }
    
    public Task BroadcastGameContinuedAsync(Guid gameId, CancellationToken cancellationToken = default) {
        string groupId = GetGroupId(gameId);
        var payload = new { GameId = gameId };  
        return _hubContext.Clients.Group(groupId).SendAsync("GameContinued", payload, cancellationToken);
    }
}

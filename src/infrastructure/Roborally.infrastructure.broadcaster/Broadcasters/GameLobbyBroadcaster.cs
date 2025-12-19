using Microsoft.AspNetCore.SignalR;
using Roborally.core.application.ApplicationContracts.Broadcasters;
using Roborally.infrastructure.broadcaster.GameLobby;

namespace Roborally.infrastructure.broadcaster.Broadcasters;

public class GameLobbyBroadcaster : IGameLobbyBroadcaster
{
    private readonly IHubContext<GameLobbyHub> _hubContext;
/// <author name="Sachin Baral 2025-09-19 13:01:07 +0200 10" />
    public GameLobbyBroadcaster(IHubContext<GameLobbyHub> hubContext)
    {
        _hubContext = hubContext;
    }

/// <author name="Sachin Baral 2025-10-01 21:53:45 +0200 15" />
    private static string GetGroupId(Guid gameId) => GroupName.GameLobby(gameId.ToString());

/// <author name="Sachin Baral 2025-09-19 13:01:07 +0200 17" />
    public async Task BroadcastUserJoinedAsync(Guid gameId, string username, CancellationToken cancellationToken = default)
    {
        string groupId = GetGroupId(gameId);
        var payload = new { GameId = gameId, Username = username };
        await _hubContext.Clients.Group(groupId).SendAsync("UserJoinedLobby", payload, cancellationToken);
    }

/// <author name="Sachin Baral 2025-09-19 13:01:07 +0200 24" />
    public async Task BroadcastUserLeftAsync(Guid gameId, string username, CancellationToken cancellationToken = default)
    {
        string groupId = GetGroupId(gameId);
        var payload = new { GameId = gameId, Username = username };
        await _hubContext.Clients.Group(groupId).SendAsync("UserLeftLobby", payload, cancellationToken);
    }
    
/// <author name="Vincenzo Altaserse 2025-10-18 13:11:59 +0200 31" />
    public async Task BroadcastHostChangedAsync(Guid gameId, string newHost, CancellationToken cancellationToken = default)
    {
        string groupId = GetGroupId(gameId);
        var payload = new { GameId = gameId, NewHost = newHost };
        await _hubContext.Clients.Group(groupId).SendAsync("HostChanged", payload, cancellationToken);
    }

/// <author name="Sachin Baral 2025-09-23 16:46:52 +0200 38" />
    public Task BroadcastGameStartedAsync(Guid gameId, CancellationToken cancellationToken = default) {
        string groupId = GetGroupId(gameId);
        var payload = new { GameId = gameId };  
        return _hubContext.Clients.Group(groupId).SendAsync("GameStarted", payload, cancellationToken);
    }
    
/// <author name="Truong Son NGO 2025-11-12 15:35:28 +0100 44" />
    public Task BroadcastGameContinuedAsync(Guid gameId, CancellationToken cancellationToken = default) {
        string groupId = GetGroupId(gameId);
        var payload = new { GameId = gameId };  
        return _hubContext.Clients.Group(groupId).SendAsync("GameContinued", payload, cancellationToken);
    }
}
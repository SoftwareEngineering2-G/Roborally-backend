using Microsoft.AspNetCore.SignalR;
using Roborally.core.application.Broadcasters;
using Roborally.infrastructure.broadcaster.Game;

namespace Roborally.infrastructure.broadcaster.Broadcasters;

public class GameBroadcaster : IGameBroadcaster{

    private readonly IHubContext<GameHub> _hubContext;

    private static string GroupName(Guid gameId) => $"game-{gameId.ToString()}";

    public GameBroadcaster(IHubContext<GameHub> hubContext) {
        this._hubContext = hubContext;
    }


    public Task BroadcastPlayerLockedInRegisterAsync(string username, Guid gameId, CancellationToken ct) {
        var payload = new {
            username,
        };
        return _hubContext.Clients.Groups(GroupName(gameId)).SendAsync("PlayerLockedInRegister", payload, ct);
    }
}
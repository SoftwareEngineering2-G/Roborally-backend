using FastEndpoints;
using Roborally.core.domain.Lobby;

namespace Roborally.core.application.DomainEventsHandlers;

public class BroadcastUserLeftLobbyHandler : IEventHandler<UserLeftLobbyEvent> 
{
    private readonly IGameLobbyBroadcaster _gameLobbyBroadcaster;

    public BroadcastUserLeftLobbyHandler(IGameLobbyBroadcaster gameLobbyBroadcaster) 
    {
        _gameLobbyBroadcaster = gameLobbyBroadcaster;
    }

    public Task HandleAsync(UserLeftLobbyEvent eventModel, CancellationToken ct) 
    {
        return _gameLobbyBroadcaster.BroadcastUserLeftAsync(eventModel.GameId, eventModel.UserUsername, ct);
    }
}

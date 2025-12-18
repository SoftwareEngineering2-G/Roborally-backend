using FastEndpoints;
using Roborally.core.application.ApplicationContracts.Broadcasters;
using Roborally.core.domain.BroadCastEvents;

namespace Roborally.infrastructure.broadcaster.BroadcastEventsHandlers;

public class GameCompletedBroadcastEventHandler : IEventHandler<GameCompletedBroadcastEvent> {
    private readonly IGameBroadcaster _gameBroadcaster;
    public GameCompletedBroadcastEventHandler(IGameBroadcaster gameBroadcaster) {
        _gameBroadcaster = gameBroadcaster;
    }


    public async Task HandleAsync(GameCompletedBroadcastEvent eventModel, CancellationToken ct) {
        await _gameBroadcaster.BroadcastGameCompletedAsync(eventModel, ct);
    }
}
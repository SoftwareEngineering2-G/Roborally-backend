using FastEndpoints;
using Roborally.core.application.ApplicationContracts.Broadcasters;
using Roborally.core.domain.BroadCastEvents;

namespace Roborally.infrastructure.broadcaster.BroadcastEventsHandlers;

public class GameCompletedBroadcastEventHandler : IEventHandler<GameCompletedBroadcastEvent> {
    private readonly IGameBroadcaster _gameBroadcaster;
/// <author name="Sachin Baral 2025-11-15 19:46:26 +0100 9" />
    public GameCompletedBroadcastEventHandler(IGameBroadcaster gameBroadcaster) {
        _gameBroadcaster = gameBroadcaster;
    }


/// <author name="Sachin Baral 2025-11-15 19:46:26 +0100 14" />
    public async Task HandleAsync(GameCompletedBroadcastEvent eventModel, CancellationToken ct) {
        await _gameBroadcaster.BroadcastGameCompletedAsync(eventModel, ct);
    }
}
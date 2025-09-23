using FastEndpoints;
using Roborally.core.domain.Lobby.DomainEvents;

namespace Roborally.core.application.DomainEventsHandlers.StartGameEvent;

public class BroadcastStartGameEventHandler : IEventHandler<GameStartedEvent> {

    private readonly IGameLobbyBroadcaster _gameLobbyBroadcaster;

    public BroadcastStartGameEventHandler(IGameLobbyBroadcaster gameLobbyBroadcaster) {
        _gameLobbyBroadcaster = gameLobbyBroadcaster;
    }

    public Task HandleAsync(GameStartedEvent eventModel, CancellationToken ct) {
        return _gameLobbyBroadcaster.BroadcastGameStartedAsync(eventModel.GameId, ct);
    }
}
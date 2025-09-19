using FastEndpoints;
using Roborally.core.domain.Lobby;

namespace Roborally.core.application.DomainEventsHandlers;

public class BroadcastUserJoinedLobbyHandler : IEventHandler<UserJoinedLobbyEvent> {
    private readonly IGameLobbyBroadcaster _gameLobbyBroadcaster;

    public BroadcastUserJoinedLobbyHandler(IGameLobbyBroadcaster gameLobbyBroadcaster) {
        _gameLobbyBroadcaster = gameLobbyBroadcaster;
    }

    public Task HandleAsync(UserJoinedLobbyEvent eventModel, CancellationToken ct) {
        Console.WriteLine("BroadcastUserJoinedLobbyHandler: Handling UserJoinedLobbyEvent");
        return _gameLobbyBroadcaster.BroadcastUserJoinedAsync(eventModel.GameId, eventModel.NewUserUsername, ct);
    }
}
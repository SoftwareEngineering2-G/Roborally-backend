using FastEndpoints;

namespace Roborally.core.domain.Lobby;

public class UserJoinedLobbyEvent : IEvent {
    public required Guid GameId { get; set; }
    public required string NewUserUsername { get; set; }

}
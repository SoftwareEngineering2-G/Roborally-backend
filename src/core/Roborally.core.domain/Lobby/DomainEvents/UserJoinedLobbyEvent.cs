using FastEndpoints;

namespace Roborally.core.domain.Lobby.DomainEvents;

public class UserJoinedLobbyEvent : IEvent {
    public required Guid GameId { get; init; }
    public required string NewUserUsername { get; init; }

}
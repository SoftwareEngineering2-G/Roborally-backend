using FastEndpoints;

namespace Roborally.core.domain.Lobby.DomainEvents;

public class GameStartedEvent : IEvent{
    public required Guid GameId { get; init; }
}
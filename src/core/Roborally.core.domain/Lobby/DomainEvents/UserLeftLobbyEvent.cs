using FastEndpoints;

namespace Roborally.core.domain.Lobby.DomainEvents;

public class UserLeftLobbyEvent : IEvent 
{
    public required Guid GameId { get; init; }
    public required string UserUsername { get; init; }
}

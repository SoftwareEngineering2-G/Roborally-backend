using FastEndpoints;

namespace Roborally.core.domain.Lobby;

public class UserLeftLobbyEvent : IEvent 
{
    public required Guid GameId { get; set; }
    public required string UserUsername { get; set; }
}

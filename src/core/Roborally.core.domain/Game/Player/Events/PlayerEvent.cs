using Roborally.core.domain.Bases;

namespace Roborally.core.domain.Game.Player.Events;

public class PlayerEvent : Event{
    public required Guid GameId { get; init; }
    public required string Username  { get;init; }
}
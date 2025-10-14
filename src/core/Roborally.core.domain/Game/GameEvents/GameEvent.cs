using Roborally.core.domain.Bases;

namespace Roborally.core.domain.Game.GameEvents;

public class GameEvent : Event{
    public required Guid GameId { get; init; }
}
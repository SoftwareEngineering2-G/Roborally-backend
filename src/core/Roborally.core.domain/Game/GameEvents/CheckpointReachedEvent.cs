using Roborally.core.domain.Game.Player.Events;

namespace Roborally.core.domain.Game.GameEvents;

public class CheckpointReachedEvent : PlayerEvent
{
    public required int CheckpointNumber { get; init; }
    
}
namespace Roborally.core.domain.Game.GameEvents;

public class CheckpointReachedEvent : GameEvent
{
    public required string Username { get; init; }
    public required int CheckpointNumber { get; init; }
}


namespace Roborally.core.domain.Game.GameEvents;

public class RoundCompletedEvent : GameEvent {
    public int CompletedRound { get; init; }
    public int NewRound { get; init; }
}
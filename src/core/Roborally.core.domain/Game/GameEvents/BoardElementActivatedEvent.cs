namespace Roborally.core.domain.Game.GameEvents;

public class BoardElementActivatedEvent : GameEvent{

    public required string BoardElementName { get; set; }
    public required int RoundCount { get; set; }
}
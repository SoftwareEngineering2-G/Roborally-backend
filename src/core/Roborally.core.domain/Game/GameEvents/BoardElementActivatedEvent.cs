namespace Roborally.core.domain.Game.GameEvents;

public class BoardElementActivatedEvent : GameEvent{

    public required string BoardElementName { get; set; }
}
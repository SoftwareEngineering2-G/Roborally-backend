using Roborally.core.domain.Game.Gameboard.BoardElement;

namespace Roborally.core.domain.Game.GameEvents;

public class BoardElementActivatedEvent : GameEvent{

    public string BoardElementName { get; set; }
}
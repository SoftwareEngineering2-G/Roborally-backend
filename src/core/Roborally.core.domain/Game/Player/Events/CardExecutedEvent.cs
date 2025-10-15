using Roborally.core.domain.Game.Deck;

namespace Roborally.core.domain.Game.Player.Events;

public sealed class CardExecutedEvent : PlayerEvent {
    public required ProgrammingCard Card { get; init; }
}

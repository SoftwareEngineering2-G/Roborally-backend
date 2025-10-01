using Roborally.core.domain.Deck;

namespace Roborally.core.domain.Game.Player.Events;

public class ProgrammingCardsDealtEvent : PlayerEvent{
    public required List<ProgrammingCard> DealtCards { get; set; }

}
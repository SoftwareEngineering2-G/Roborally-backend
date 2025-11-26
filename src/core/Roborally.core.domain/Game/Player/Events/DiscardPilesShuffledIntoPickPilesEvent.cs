using Roborally.core.domain.Game.Deck;

namespace Roborally.core.domain.Game.Player.Events;

public class DiscardPilesShuffledIntoPickPilesEvent : PlayerEvent {
    public required List<ProgrammingCard> NewPickPiles { get; set; }
}
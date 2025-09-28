using Roborally.core.domain.Deck;

namespace Roborally.core.domain.Game.Player.Events;

public sealed class RegistersProgrammedEvent : PlayerEvent{
    public required List<ProgrammingCard> ProgrammedCardsInOrder { get; set; }
    
}
using Roborally.core.domain.Game.Deck;

namespace Roborally.core.domain.Game.GameEvents;

public class DealCardInfo
{
    public List<ProgrammingCard> DealtCards { get; set; }
    public bool IsDeckReshuffled { get; set; }
}
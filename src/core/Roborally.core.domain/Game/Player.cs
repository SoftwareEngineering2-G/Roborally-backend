using Roborally.core.domain.Deck;

namespace Roborally.core.domain.Game;

public class Player {
    public required User.User User { get; set; }
    public required Robot Robot { get; set; }

    public required Direction CurrentFacingDirection { get; set; }
    public required Position CurrentPosition { get; set; }

    public ProgrammingDeck ProgrammingDeck { get; set; }


}
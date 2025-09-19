using Roborally.core.domain.Bases;

namespace Roborally.core.domain.Deck;

public class ProgrammingCard : Enumeration
{
    public static readonly ProgrammingCard Move1 = new ProgrammingCard("Move 1");
    public static readonly ProgrammingCard Move2 = new ProgrammingCard("Move 2");
    public static readonly ProgrammingCard Move3 = new ProgrammingCard("Move 3");
    public static readonly ProgrammingCard RotateLeft = new ProgrammingCard("Rotate Left");
    public static readonly ProgrammingCard RotateRight = new ProgrammingCard("Rotate Right");
    public static readonly ProgrammingCard UTurn = new ProgrammingCard("U-Turn");
    public static readonly ProgrammingCard MoveBack = new ProgrammingCard("Move Back");
    public static readonly ProgrammingCard PowerUp = new ProgrammingCard("Power Up");
    public static readonly ProgrammingCard Again = new ProgrammingCard("Again");

    private ProgrammingCard(string displayName) : base(displayName)
    {
    }
}
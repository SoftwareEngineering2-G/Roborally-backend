using System.Reflection;
using Roborally.core.domain.Bases;

namespace Roborally.core.domain.Game.Deck;

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
    public static readonly ProgrammingCard SwapPosition = new ProgrammingCard("Swap Position");
    public static readonly ProgrammingCard MovementChoice = new ProgrammingCard("Movement Choice");

    private ProgrammingCard(string displayName) : base(displayName)
    {
    }

    public static ProgrammingCard FromString(string cardName)
    {
        // Use reflection to get all static readonly ProgrammingCard fields
        var fields = typeof(ProgrammingCard)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.FieldType == typeof(ProgrammingCard))
            .Select(f => f.GetValue(null) as ProgrammingCard)
            .Where(card => card != null);

        var matchingCard = fields.FirstOrDefault(card => card!.DisplayName == cardName);
        
        return matchingCard ?? throw new ArgumentException($"Unknown card name: {cardName}");
    }
}

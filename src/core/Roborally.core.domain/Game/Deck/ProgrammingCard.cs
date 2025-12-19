using System.Reflection;
using Roborally.core.domain.Bases;

namespace Roborally.core.domain.Game.Deck;

public class ProgrammingCard : Enumeration
{
/// <author name="Sachin Baral 2025-09-16 15:57:05 +0200 8" />
    public static readonly ProgrammingCard Move1 = new ProgrammingCard("Move 1");
/// <author name="Sachin Baral 2025-09-16 15:57:05 +0200 9" />
    public static readonly ProgrammingCard Move2 = new ProgrammingCard("Move 2");
/// <author name="Sachin Baral 2025-09-16 15:57:05 +0200 10" />
    public static readonly ProgrammingCard Move3 = new ProgrammingCard("Move 3");
/// <author name="Sachin Baral 2025-09-16 15:57:05 +0200 11" />
    public static readonly ProgrammingCard RotateLeft = new ProgrammingCard("Rotate Left");
/// <author name="Sachin Baral 2025-09-16 15:57:05 +0200 12" />
    public static readonly ProgrammingCard RotateRight = new ProgrammingCard("Rotate Right");
/// <author name="Sachin Baral 2025-09-16 15:57:05 +0200 13" />
    public static readonly ProgrammingCard UTurn = new ProgrammingCard("U-Turn");
/// <author name="Sachin Baral 2025-09-16 15:57:05 +0200 14" />
    public static readonly ProgrammingCard MoveBack = new ProgrammingCard("Move Back");
/// <author name="Sachin Baral 2025-09-16 15:57:05 +0200 15" />
    public static readonly ProgrammingCard Again = new ProgrammingCard("Again");
/// <author name="Satish 2025-11-24 10:20:04 +0100 16" />
    public static readonly ProgrammingCard SwapPosition = new ProgrammingCard("Swap Position");
/// <author name="Satish 2025-11-24 10:20:04 +0100 17" />
    public static readonly ProgrammingCard MovementChoice = new ProgrammingCard("Movement Choice");

/// <author name="Truong Son NGO 2025-09-19 17:04:26 +0200 19" />
    private ProgrammingCard(string displayName) : base(displayName)
    {
    }

/// <author name="Sachin Baral 2025-09-28 13:55:14 +0200 23" />
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
namespace Roborally.core.domain.Game.Deck;

public class ProgrammingDeck {
    public List<ProgrammingCard> PickPiles { get; set; }
    public List<ProgrammingCard> DiscardedPiles { get; set; }

    // Fisher-Yates shuffle utility
/// <author name="Sachin Baral 2025-10-01 21:53:45 +0200 8" />
    private static void ShuffleList<T>(List<T> list) {
        var rng = Random.Shared;
        for (int i = list.Count - 1; i > 0; i--) {
            int j = rng.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    // Creates a new shuffled programming deck
/// <author name="Sachin Baral 2025-09-28 13:55:14 +0200 17" />
    public static ProgrammingDeck NewShuffled() {
        List<ProgrammingCard> cards = new List<ProgrammingCard>(21);
        // 4 copies of Move 1, rotate right, rotate left
        for (int i = 0; i < 4; i++) {
            cards.Add(ProgrammingCard.Move1);
            cards.Add(ProgrammingCard.RotateLeft);
            cards.Add(ProgrammingCard.RotateRight);
        }
        // 3 copies of Move 2
        for (int i = 0; i < 3; i++) {
            cards.Add(ProgrammingCard.Move2);
        }
        cards.Add(ProgrammingCard.Move3);
        cards.Add(ProgrammingCard.UTurn);
        cards.Add(ProgrammingCard.MoveBack);
        cards.Add(ProgrammingCard.Again);
        cards.Add(ProgrammingCard.SwapPosition);
        cards.Add(ProgrammingCard.MovementChoice);
        ShuffleList(cards);
        return new ProgrammingDeck(cards);
    }

/// <author name="Sachin Baral 2025-09-28 13:55:14 +0200 39" />
    private ProgrammingDeck(List<ProgrammingCard> cards) {
        PickPiles = cards;
        DiscardedPiles = [];
    }

/// <author name="Sachin Baral 2025-09-28 13:55:14 +0200 44" />
    private ProgrammingDeck() {
        // For EF Core
    }

    // Draw up to 'count' cards from PickPiles, removing them from the deck
/// <author name="Sachin Baral 2025-10-01 21:53:45 +0200 49" />
    public List<ProgrammingCard> Draw(int count) {
        int take = Math.Min(count, PickPiles.Count);
        var drawn = PickPiles.Take(take).ToList();
        PickPiles = PickPiles.Skip(take).ToList();
        return drawn;
    }

    // Shuffle DiscardedPiles into PickPiles and clear DiscardedPiles
/// <author name="Sachin Baral 2025-10-01 21:53:45 +0200 57" />
    public void RefillFromDiscard() {
        ShuffleList(DiscardedPiles);
        PickPiles = DiscardedPiles;
        DiscardedPiles = [];
    }
}
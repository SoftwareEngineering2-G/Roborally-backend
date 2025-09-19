namespace Roborally.core.domain.Deck;

public class ProgrammingDeck
{
    public List<ProgrammingCard> ProgrammingCards { get; init; }

    // Creates a new shuffled programming deck
    public static ProgrammingDeck New()
    {
        List<ProgrammingCard> cards = new List<ProgrammingCard>(20);
        // 4 copies of Move 1, rotate right, rotate left

        for (int i = 0; i < 4; i++)
        {
            cards.Add(ProgrammingCard.Move1);
            cards.Add(ProgrammingCard.RotateLeft);
            cards.Add(ProgrammingCard.RotateRight);
        }

        // 3 copies of Move 2
        for (int i = 0; i < 3; i++)
        {
            cards.Add(ProgrammingCard.Move2);
        }

        cards.Add(ProgrammingCard.Move3);
        cards.Add(ProgrammingCard.UTurn);
        cards.Add(ProgrammingCard.MoveBack);
        cards.Add(ProgrammingCard.PowerUp);
        cards.Add(ProgrammingCard.Again);

        // Shuffle the cards using Fisher-Yates algorithm
        for (int i = cards.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Shared.Next(i + 1);
            (cards[i], cards[randomIndex]) = (cards[randomIndex], cards[i]);
        }

        return new ProgrammingDeck(cards);
    }

    private ProgrammingDeck(List<ProgrammingCard> cards)
    {
        ProgrammingCards = cards;
    }
}
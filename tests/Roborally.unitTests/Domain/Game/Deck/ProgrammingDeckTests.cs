using Roborally.core.domain.Game.Deck;

namespace Roborally.unitTests.Domain.Deck;

public class ProgrammingDeckTests
{
    [Fact]
    public void RefillFromDiscard_RemovesDiscardedPiles_AndAddsPickPiles()
    {
        // Arrange
        var programmingDeck = ProgrammingDeck.NewShuffled();
        programmingDeck.DiscardedPiles.AddRange(programmingDeck.PickPiles);
        programmingDeck.PickPiles.Clear();
        
        // Act
        programmingDeck.RefillFromDiscard();
        
        // Assert
        Assert.Empty(programmingDeck.DiscardedPiles);
        Assert.NotEmpty(programmingDeck.PickPiles);
    }
}
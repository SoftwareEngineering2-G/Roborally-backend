using Roborally.core.domain.Game;
using Roborally.core.domain.Game.Deck;
using Roborally.core.domain.Game.Player;
using Roborally.core.domain.Game.Player.Events;
using Roborally.unitTests.Factory;

namespace Roborally.unitTests.Domain;

public class GameQueryExtensionsTests
{
    [Fact]
    public void GetRevealedCardsDisplayNames_ShouldReturnRevealedCards_WhenProgrammedEventPresent()
    {
        // Arrange
        var game = GameFactory.GetValidGame();
        var player = game.Players[0];
        var round = 1;

        var programmedEvent = new RegistersProgrammedEvent
        {
            GameId = game.GameId,
            Username = player.Username,
            Round = round,
            HappenedAt = DateTime.UtcNow,
            ProgrammedCardsInOrder = [
                ProgrammingCard.Move1, 
                ProgrammingCard.RotateLeft, 
                ProgrammingCard.Move2, 
                ProgrammingCard.UTurn, 
                ProgrammingCard.Move3
            ]
        };
        player.PlayerEvents.Add(programmedEvent);

        var currentRevealedRegister = 3;

        // Act
        var revealedCards = player.GetRevealedCardsDisplayNames(round, currentRevealedRegister);

        // Assert
        Assert.Equal(3, revealedCards.Count);
    }

    [Fact]
    public void GetLastCardExecutedEvent_ReturnsEvent_WhenRegisterSameAsEventsThisRound()
    {
        // Arrange
        var game = GameFactory.GetValidGame();
        var player = game.Players[0];
        var round = 1;
        var register = 1;

        var cardExecutedEvent = new CardExecutedEvent
        {
            Card = ProgrammingCard.Move1,
            GameId = game.GameId,
            HappenedAt = DateTime.UtcNow,
            Round = round,
            Username = player.Username
        };
        player.PlayerEvents.Add(cardExecutedEvent);
        
        // Act
        var lastCardExecutedEvent = player.GetLastCardExecutedEvent(round, register);
        
        // Assert
        Assert.Equal(cardExecutedEvent, lastCardExecutedEvent);
    }

    [Fact]
    public void GetLastPlayerToExecuteCard_ReturnsPlayer_WhenPlayerWithExecutionExists()
    {
        // Arrange
        var game = GameFactory.GetValidGame();
        var player = game.Players[0];
        var round = 1;
        var register = 1;

        var cardExecutedEvent = new CardExecutedEvent
        {
            Username = player.Username,
            GameId = game.GameId,
            HappenedAt = DateTime.UtcNow,
            Card = ProgrammingCard.Move1,
            Round = round
        };
        player.PlayerEvents.Add(cardExecutedEvent);
        
        // Act
        var lastPlayerToExecuteCard = game.GetLastPlayerToExecuteCard(round, register);
        
        // Assert
        Assert.Equal(player, lastPlayerToExecuteCard);
    }
}
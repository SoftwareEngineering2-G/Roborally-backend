using Moq;
using Roborally.core.domain.Bases;
using Roborally.core.domain.Game;
using Roborally.core.domain.Game.Deck;
using Roborally.core.domain.Game.Player.Events;
using Roborally.unitTests.Factory;

namespace Roborally.unitTests.Domain;

public class GameQueryExtensionsTests
{
    private readonly Mock<ISystemTime> _systemTimeMock;

    public GameQueryExtensionsTests()
    {
        _systemTimeMock = new Mock<ISystemTime>();
        _systemTimeMock.Setup(x => x.CurrentTime).Returns(DateTime.UtcNow);
    }
    
    [Fact]
    public void GetDealtCardsDisplayNames_ShouldReturnNull_WhenNoCardsDealtEventPresent()
    {
        // Arrange
        var game = GameFactory.GetValidGame();
        var player = game.Players[0];
        var round = 1;

        // Act
        var dealtCards = player.GetDealtCardsDisplayNames(round);

        // Assert
        Assert.Null(dealtCards);
    }

    [Fact]
    public void GetDealtCardsDisplayNames_ShouldReturnDealtCards_WhenCardsDealtEventPresent()
    {
        // Arrange
        var game = GameFactory.GetValidGame();
        var player = game.Players[0];
        var round = 1;
        game.DealDecksToAllPlayers(_systemTimeMock.Object);
        var dealtCards = player.GetCardsDealtEvent(round)!.DealtCards;
        
        // Act
        var dealtCardsNames = player.GetDealtCardsDisplayNames(round);

        // Assert
        Assert.NotNull(dealtCardsNames);
        Assert.Equal(dealtCards.Count, dealtCardsNames.Count);
    }

    [Fact]
    public void GetProgrammedRegistersDisplayNames_ShouldReturnNull_WhenNoProgrammedEventPresent()
    {
        // Arrange
        var game = GameFactory.GetValidGame();
        var player = game.Players[0];
        var round = 1;

        // Act
        var programmedCards = player.GetProgrammedRegistersDisplayNames(round);

        // Assert
        Assert.Null(programmedCards);
    }

    [Fact]
    public void GetProgrammedRegistersDisplayNames_ShouldReturnProgrammedCards_WhenProgrammedEventPresent()
    {
        // Arrange
        var game = GameFactory.GetValidGame();
        game.CurrentPhase = GamePhase.ProgrammingPhase;
        game.DealDecksToAllPlayers(_systemTimeMock.Object);
        var round = 1;
        var player = game.Players[0];
        var lockedInCards = player.GetCardsDealtEvent(round)!.DealtCards.Take(5).ToList();
        
        game.LockInRegisters(player.Username, lockedInCards, _systemTimeMock.Object);
        
        // Act
        var programmedRegisters = player.GetProgrammedRegistersDisplayNames(round);
        
        // Assert
        Assert.NotNull(programmedRegisters);
        Assert.Equal(5, programmedRegisters.Count);
    }

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
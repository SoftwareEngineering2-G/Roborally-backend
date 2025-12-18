using System.Reflection;
using Moq;
using Roborally.core.domain;
using Roborally.core.domain.Bases;
using Roborally.core.domain.Game;
using Roborally.core.domain.Game.Deck;
using Roborally.core.domain.Game.Player;
using Roborally.unitTests.Factory;

namespace Roborally.unitTests.Domain.PlayerTests;

public class PlayerTests
{
    private readonly Mock<ISystemTime> _systemTimeMock;
    private readonly MethodInfo _lockInRegisterMethod;
    private readonly List<ProgrammingCard> _programmingCards;

    public PlayerTests()
    {
        _systemTimeMock = new Mock<ISystemTime>();
        _systemTimeMock.Setup(x => x.CurrentTime).Returns(DateTime.UtcNow);

        _lockInRegisterMethod = typeof(Player).GetMethod("LockInRegisters",
            BindingFlags.NonPublic | BindingFlags.Instance)!;

        _programmingCards =
        [
            ProgrammingCard.Move1, ProgrammingCard.Move2, ProgrammingCard.Move3,
            ProgrammingCard.RotateLeft, ProgrammingCard.RotateRight,
            ProgrammingCard.UTurn,
            ProgrammingCard.MoveBack,
            ProgrammingCard.Again,
            ProgrammingCard.MovementChoice,
            ProgrammingCard.SwapPosition
        ];
    }

    [Fact]
    public void LockInRegisters_ShouldThrow_WhenLessThanFiveCardsLocked()
    {
        // Arrange
        var player = PlayerFactory.GetValidPlayer();
        var lockedInCards = new List<ProgrammingCard>()
        {
            ProgrammingCard.Move1,
            ProgrammingCard.Move1,
            ProgrammingCard.Move1,
        };

        // Act & Assert
        var exception = Assert.Throws<TargetInvocationException>(() =>
            _lockInRegisterMethod.Invoke(player, [lockedInCards, _systemTimeMock.Object]));
    
        Assert.IsType<CustomException>(exception.InnerException);
    }

    [Fact]
    public void LockInRegisters_ShouldThrow_WhenNoCardsDealt()
    {
        // Arrange
        var player = PlayerFactory.GetValidPlayer();
        var lockedInCards = new List<ProgrammingCard>()
        {
            ProgrammingCard.Move1,
            ProgrammingCard.Move1,
            ProgrammingCard.Move1,
            ProgrammingCard.Move1,
            ProgrammingCard.Move1,
        };

        // Act & Assert
        var exception = Assert.Throws<TargetInvocationException>(() =>
            _lockInRegisterMethod.Invoke(player, [lockedInCards, _systemTimeMock.Object]));
        
        Assert.IsType<CustomException>(exception.InnerException);
    }

    [Fact]
    public void LockInRegisters_ShouldThrow_WhenLockedCardsNotDealt()
    {
        // Arrange
        var game = GameFactory.GetValidGame();
        game.RoundCount = 1;
        var player = game.Players[0];
        game.DealDecksToAllPlayers(_systemTimeMock.Object);
        var dealtCards = player.GetCardsDealtEvent(1)!.DealtCards;

        var notDealtCard = _programmingCards.First(c => !dealtCards.Contains(c));

        var lockedInCards = new List<ProgrammingCard>()
        {
            ProgrammingCard.Move1,
            ProgrammingCard.Move1,
            ProgrammingCard.Move1,
            ProgrammingCard.Move1,
            notDealtCard
        };

        // Act & Assert
        var exception = Assert.Throws<TargetInvocationException>(() =>
            _lockInRegisterMethod.Invoke(player, [lockedInCards, _systemTimeMock.Object]));
        
        Assert.IsType<CustomException>(exception.InnerException);
    }

    [Fact]
    public void LockInRegisters_ShouldThrow_WhenAlreadyLockedIn()
    {
        // Arrange
        var game = GameFactory.GetValidGame();
        const int round = 1;
        game.RoundCount = round;
        var player = game.Players[0];
        game.DealDecksToAllPlayers(_systemTimeMock.Object);
        var dealtCards = player.GetCardsDealtEvent(round)!.DealtCards;
        _lockInRegisterMethod.Invoke(player, [dealtCards.Take(5).ToList(), _systemTimeMock.Object]);

        // Act & Assert
        var exception = Assert.Throws<TargetInvocationException>(() =>
            _lockInRegisterMethod.Invoke(player, [dealtCards, _systemTimeMock.Object]));
        
        Assert.IsType<Exception>(exception.InnerException);
    }
}
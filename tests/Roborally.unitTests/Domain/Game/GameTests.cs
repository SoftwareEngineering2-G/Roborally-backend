using Moq;
using Roborally.core.domain;
using Roborally.core.domain.Bases;
using Roborally.core.domain.Game;
using Roborally.core.domain.Game.Deck;
using Roborally.core.domain.Game.GameEvents;
using Roborally.core.domain.Game.Player;
using Roborally.unitTests.Factory;

namespace Roborally.unitTests.Domain;

public class GameTests
{
    private core.domain.Game.Game _game;
    private readonly Mock<ISystemTime> _systemTimeMock;

    public GameTests()
    {
        _game = GameFactory.GetValidGame();
        _systemTimeMock = new Mock<ISystemTime>();
        _systemTimeMock.Setup(x => x.CurrentTime).Returns(DateTime.UtcNow);
    }

    [Fact]
    public void DealDecksToAllPlayers_ShouldDealDecks_WhenGameInProgrammingPhase()
    {
        // Arrange
        _game = GameFactory.GetValidGame();
        _game.CurrentPhase = GamePhase.ProgrammingPhase;
        var initialCardsCount = _game.Players[0].ProgrammingDeck.PickPiles.Count;

        // Act
        _game.DealDecksToAllPlayers(_systemTimeMock.Object);

        // Assert
        foreach (var player in _game.Players)
        {
            Assert.True(player.ProgrammingDeck.PickPiles.Count < initialCardsCount);
        }
    }

    [Fact]
    public void LockInRegisters_ShouldThrow_WhenGameInActivationPhase()
    {
        // Arrange
        _game.CurrentPhase = GamePhase.ActivationPhase;

        // Act & Assert
        Assert.Throws<CustomException>(() => _game.LockInRegisters(_game.Players[0].Username,
            It.IsAny<List<ProgrammingCard>>(),
            It.IsAny<ISystemTime>()));
    }

    [Fact]
    public void LockInRegisters_ShouldThrow_WhenGamePaused()
    {
        // Arrange
        _game.CurrentPhase = GamePhase.ProgrammingPhase;
        _game.IsPaused = true;

        // Act & Assert
        Assert.Throws<CustomException>(() => _game.LockInRegisters(_game.Players[0].Username,
            It.IsAny<List<ProgrammingCard>>(),
            It.IsAny<ISystemTime>()));
    }

    [Fact]
    public void LockInRegisters_ShouldThrow_WhenPlayerDoesNotExist()
    {
        // Arrange
        _game.CurrentPhase = GamePhase.ProgrammingPhase;
        _game.IsPaused = false;

        string nonExistingPlayer;

        do
        {
            nonExistingPlayer = Guid.NewGuid().ToString();
        } while (_game.Players.Any(x => x.Username == nonExistingPlayer));

        // Act & Assert
        Assert.Throws<CustomException>(() => _game.LockInRegisters(nonExistingPlayer,
            It.IsAny<List<ProgrammingCard>>(),
            It.IsAny<ISystemTime>()));
    }

    [Fact]
    public void LockInRegisters_ShouldLockInRegisters_WhenPlayerExists()
    {
        // Arrange
        _game.CurrentPhase = GamePhase.ProgrammingPhase;
        _game.IsPaused = false;

        var player = _game.Players[0];
        _game.DealDecksToAllPlayers(_systemTimeMock.Object);
        var cardsToLockIn = player.GetCardsDealtEvent(_game.RoundCount)!.DealtCards.Take(5).ToList();

        // Act
        _game.LockInRegisters(player.Username, cardsToLockIn, _systemTimeMock.Object);

        // Assert
        var lockedInEvent = player.GetRegistersProgrammedEvent(_game.RoundCount);
        Assert.NotNull(lockedInEvent);
        Assert.Equal(cardsToLockIn, lockedInEvent.ProgrammedCardsInOrder);
    }

    [Fact]
    public void RevealNextRegister_ShouldThrow_WhenGameNotInActivationPhase()
    {
        // Arrange
        _game.CurrentPhase = GamePhase.ProgrammingPhase;

        // Act & Assert
        Assert.Throws<CustomException>(() => _game.RevealNextRegister());
    }

    [Fact]
    public void RevealNextRegister_ShouldThrow_WhenGamePaused()
    {
        // Arrange
        _game.CurrentPhase = GamePhase.ActivationPhase;
        _game.IsPaused = true;

        // Act & Assert
        Assert.Throws<CustomException>(() => _game.RevealNextRegister());
    }

    [Fact]
    public void RevealNextRegister_ShouldThrow_WhenAllRegistersRevealed()
    {
        // Arrange
        _game.CurrentPhase = GamePhase.ActivationPhase;
        _game.IsPaused = false;
        _game.CurrentRevealedRegister = 5;

        // Act & Assert
        Assert.Throws<CustomException>(() => _game.RevealNextRegister());
    }

    [Fact]
    public void RevealNextRegister_ShouldRevealNextRegister_WhenConditionsMet()
    {
        // Arrange
        _game.CurrentPhase = GamePhase.ProgrammingPhase;
        _game.DealDecksToAllPlayers(_systemTimeMock.Object);
        foreach (var gamePlayer in _game.Players)
        {
            _game.LockInRegisters(gamePlayer.Username,
                gamePlayer.GetCardsDealtEvent(_game.RoundCount)!.DealtCards.Take(5).ToList(),
                _systemTimeMock.Object);
        }

        _game.CurrentPhase = GamePhase.ActivationPhase;
        _game.IsPaused = false;
        _game.CurrentRevealedRegister = 2;

        // Act
        _game.RevealNextRegister();

        // Assert
        Assert.Equal(3, _game.CurrentRevealedRegister);
    }

    [Fact]
    public async Task ActivateNextBoardElement_ShouldThrow_WhenGameNotInActivationPhase()
    {
        // Arrange
        _game.CurrentPhase = GamePhase.ProgrammingPhase;

        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => _game.ActivateNextBoardElement(_systemTimeMock.Object));
    }

    [Fact]
    public async Task ActivateNextBoardElement_ShouldThrow_WhenGamePaused()
    {
        // Arrange
        _game.CurrentPhase = GamePhase.ActivationPhase;
        _game.IsPaused = true;

        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => _game.ActivateNextBoardElement(_systemTimeMock.Object));
    }

    [Fact]
    public void ExecuteProgrammingCard_ShouldThrow_WhenGameNotInActivationPhase()
    {
        // Arrange
        _game.CurrentPhase = GamePhase.ProgrammingPhase;

        // Act & Assert
        Assert.Throws<CustomException>(() => _game.ExecuteProgrammingCard(
            _game.Players[0].Username,
            It.IsAny<ProgrammingCard>(),
            _systemTimeMock.Object));
    }

    [Fact]
    public void ExecuteProgrammingCard_ShouldThrow_WhenGamePaused()
    {
        // Arrange
        _game.CurrentPhase = GamePhase.ActivationPhase;
        _game.IsPaused = true;

        // Act & Assert
        Assert.Throws<CustomException>(() => _game.ExecuteProgrammingCard(
            _game.Players[0].Username,
            It.IsAny<ProgrammingCard>(),
            _systemTimeMock.Object));
    }

    [Fact]
    public void ExecuteProgrammingCard_ShouldThrow_WhenPlayerDoesNotExist()
    {
        // Arrange
        _game.CurrentPhase = GamePhase.ActivationPhase;
        _game.IsPaused = false;

        string nonExistingPlayer;
        do
        {
            nonExistingPlayer = Guid.NewGuid().ToString();
        } while (_game.Players.Any(x => x.Username == nonExistingPlayer));

        // Act & Assert
        Assert.Throws<CustomException>(() => _game.ExecuteProgrammingCard(
            nonExistingPlayer,
            It.IsAny<ProgrammingCard>(),
            _systemTimeMock.Object));
    }

    [Fact]
    public void ExecuteProgrammingCard_ShouldExecuteCard_WhenConditionsMet()
    {
        // Arrange
        _game.CurrentPhase = GamePhase.ActivationPhase;
        _game.IsPaused = false;

        var player = _game.Players[0];
        player.CurrentPosition = new Position(0, 0);
        player.CurrentFacingDirection = Direction.South;
        var card = ProgrammingCard.Move1;

        // Act
        _game.ExecuteProgrammingCard(player.Username, card, _systemTimeMock.Object);

        // Assert
        Assert.Equal(new Position(0, 1), player.CurrentPosition);
    }

    [Fact]
    public void RequestPauseGame_ShouldThrow_WhenGamePaused()
    {
        // Arrange
        _game.IsPaused = true;

        // Act & Assert
        Assert.Throws<CustomException>(() => _game.RequestPauseGame(_game.Players[0].Username, _systemTimeMock.Object));
    }

    [Fact]
    public void RequestPauseGame_ShouldThrow_WhenPlayerDoesNotExist()
    {
        // Arrange
        _game.IsPaused = false;

        string nonExistingPlayer;
        do
        {
            nonExistingPlayer = Guid.NewGuid().ToString();
        } while (_game.Players.Any(x => x.Username == nonExistingPlayer));

        // Act & Assert
        Assert.Throws<CustomException>(() => _game.RequestPauseGame(nonExistingPlayer, _systemTimeMock.Object));
    }

    [Fact]
    public void RequestPauseGame_ShouldAddPauseGameEvent_WhenConditionsMet()
    {
        // Arrange
        _game.GameEvents.Clear();
        _game.IsPaused = false;
        var player = _game.Players[0];

        // Act
        _game.RequestPauseGame(player.Username, _systemTimeMock.Object);

        // Assert
        Assert.Single(_game.GameEvents);
        Assert.IsType<PauseGameEvent>(_game.GameEvents[0]);
    }

    [Fact]
    public void ResponsePauseGame_ShouldThrow_WhenPlayerDoesNotExist()
    {
        // Arrange
        string nonExistingPlayer;
        do
        {
            nonExistingPlayer = Guid.NewGuid().ToString();
        } while (_game.Players.Any(x => x.Username == nonExistingPlayer));

        // Act & Assert
        Assert.Throws<CustomException>(() => _game.ResponsePauseGame(nonExistingPlayer, true, _systemTimeMock.Object));
    }

    [Fact]
    public void ResponsePauseGame_ShouldAddPauseGameEvent_WhenConditionsMet()
    {
        // Arrange
        _game.GameEvents.Clear();
        var player = _game.Players[0];

        // Act
        _game.ResponsePauseGame(player.Username, true, _systemTimeMock.Object);

        // Assert
        Assert.Single(_game.GameEvents);
        Assert.IsType<PauseGameEvent>(_game.GameEvents[0]);
    }

    [Fact]
    public void GetGamePauseState_ShouldThrow_WhenNoPauseRequest()
    {
        // Arrange
        _game.GameEvents.Clear();

        // Act & Assert
        Assert.Throws<CustomException>(() => _game.GetGamePauseState());
    }

    [Fact]
    public void GetGamePauseState_ShouldReturnNull_WhenNotEveryPlayerResponded()
    {
        // Arrange
        _game = GameFactory.GetValidGame(2);
        _game.IsPaused = false;
        var requester = _game.Players[0];
        _game.RequestPauseGame(requester.Username, _systemTimeMock.Object);

        // Act
        var gamePauseState = _game.GetGamePauseState();

        // Assert
        Assert.Null(gamePauseState);
    }

    [Fact]
    public void GetGamePauseState_ShouldReturnPauseState_WhenEveryPlayerResponded()
    {
        // Arrange
        _game = GameFactory.GetValidGame(2);
        _game.IsPaused = false;
        var requester = _game.Players[0];
        var responder = _game.Players[1];
        _game.RequestPauseGame(requester.Username, _systemTimeMock.Object);
        _systemTimeMock.Setup(x => x.CurrentTime).Returns(DateTime.UtcNow.AddSeconds(1));
        _game.ResponsePauseGame(responder.Username, true, _systemTimeMock.Object);

        // Act
        var gamePauseState = _game.GetGamePauseState();

        // Assert
        Assert.NotNull(gamePauseState);
    }

    [Fact]
    public void ContinueGame_ShouldThrow_WhenGameNotPaused()
    {
        // Arrange
        _game.IsPaused = false;

        // Act & Assert
        Assert.Throws<CustomException>(() => _game.ContinueGame());
    }
    
    [Fact]
    public void ContinueGame_ShouldContinueGame_WhenGameIsPaused()
    {
        // Arrange
        _game.IsPaused = true;

        // Act
        _game.ContinueGame();

        // Assert
        Assert.False(_game.IsPaused);
    }

    [Fact]
    public void GetCurrentExecutingRegister_ShouldReturnNull_WhenGameNotInActivationPhase()
    {
        // Arrange
        _game.CurrentPhase = GamePhase.ProgrammingPhase;

        // Act & Assert
        Assert.Null(_game.GetCurrentExecutingRegister());
    }

    [Fact]
    public void GetCurrentExecutingRegister_ShouldReturnCurrentRevealedRegister_WhenGameInActivationPhase()
    {
        // Arrange
        _game.CurrentPhase = GamePhase.ActivationPhase;
        _game.CurrentRevealedRegister = 2;

        // Act
        var currentRegister = _game.GetCurrentExecutingRegister();

        // Assert
        Assert.NotNull(currentRegister);
        Assert.Equal(2, currentRegister);
    }

    [Fact]
    public void DealDecksToAllPlayers_ShouldThrow_WhenGameNotInProgrammingPhase()
    {
        // Arrange
        _game.CurrentPhase = GamePhase.ActivationPhase;

        // Act & Assert
        Assert.Throws<CustomException>(() => _game.DealDecksToAllPlayers(It.IsAny<ISystemTime>()));
    }

    [Fact]
    public void GetNextExecutingPlayer_ShouldReturnNull_WhenGameNotInActivationPhase()
    {
        // Arrange
        _game.CurrentPhase = GamePhase.ProgrammingPhase;

        // Act & Assert
        Assert.Null(_game.GetNextExecutingPlayer());
    }

    [Fact]
    public void GetNextExecutingPlayer_ShouldReturnNull_WhenNoRevealedRegisters()
    {
        // Arrange
        _game.CurrentPhase = GamePhase.ActivationPhase;
        _game.CurrentRevealedRegister = 0;

        // Act & Assert
        Assert.Null(_game.GetNextExecutingPlayer());
    }

    [Fact]
    public void GetNextExecutingPlayer_ShouldReturnFirstPlayer_WhenNoPlayerHasExecutedCurrentRegister()
    {
        // Arrange
        _game = GameFactory.GetValidGame(2);
        _game.CurrentPhase = GamePhase.ActivationPhase;
        _game.CurrentRevealedRegister = 1;
        _game.RoundCount = 1;

        var firstPlayer = _game.Players[0];
        var secondPlayer = _game.Players[1];

        // Act
        var nextPlayer = _game.GetNextExecutingPlayer();

        // Assert
        Assert.NotNull(nextPlayer);
        Assert.Equal(firstPlayer.Username, nextPlayer.Username);
    }

    [Fact]
    public void GetNextExecutingPlayer_ShouldReturnNextPlayer_WhenPlayerExist()
    {
        // Arrange
        _game = GameFactory.GetValidGame(2);
        _game.CurrentPhase = GamePhase.ActivationPhase;
        _game.CurrentRevealedRegister = 1;
        _game.RoundCount = 1;

        var firstPlayer = _game.Players[0];
        firstPlayer.RoundCount = 1;
        firstPlayer.RecordCardExecution(It.IsAny<ProgrammingCard>(), _systemTimeMock.Object);
        var secondPlayer = _game.Players[1];

        // Act
        var nextPlayer = _game.GetNextExecutingPlayer();

        // Assert
        Assert.NotNull(nextPlayer);
        Assert.Equal(secondPlayer.Username, nextPlayer.Username);
    }
}
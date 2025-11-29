using Moq;
using Roborally.core.domain;
using Roborally.core.domain.Bases;
using Roborally.core.domain.Game.CardActions;
using Roborally.core.domain.Game.Deck;
using Roborally.core.domain.Game.Player;
using Roborally.core.domain.Game.Player.Events;
using Roborally.unitTests.Factory;

namespace Roborally.unitTests.Domain.Game.ActionCardTest;

public class AgainCardActionTests
{
    private readonly core.domain.Game.Game _game;
    private readonly Player _player;
    private readonly AgainCardAction _action;
    private readonly Mock<ISystemTime> _systemTimeMock;

    public AgainCardActionTests()
    {
        _game = GameFactory.GetValidGame();
        _player = _game.Players[0];
        _action = new AgainCardAction();
        _systemTimeMock = new Mock<ISystemTime>();
        _systemTimeMock.Setup(x => x.CurrentTime).Returns(DateTime.UtcNow);
    }

    [Fact]
    public void Execute_ShouldThrow_WhenNoPreviousCard()
    {
        var exception = Assert.Throws<CustomException>(() =>
            _action.Execute(_player, _game, _systemTimeMock.Object, null));

        Assert.Contains("No previous action to repeat", exception.Message);
        Assert.Equal(400, exception.StatusCode);
    }
    
    [Fact]
    public void Execute_ShouldThrow_WhenPreviousCardIsAgain()
    {
        _player.PlayerEvents.Add(new CardExecutedEvent
        {
            GameId = _game.GameId,
            HappenedAt = DateTime.UtcNow,
            Round = 1,
            Username = _player.Username,
            Card = ProgrammingCard.Again
        });

        var exception = Assert.Throws<CustomException>(() =>
            _action.Execute(_player, _game, _systemTimeMock.Object, null));

        Assert.Contains("Cannot repeat an Again action", exception.Message);
        Assert.Equal(400, exception.StatusCode);
    }

    [Fact]
    public void Execute_ShouldThrow_WhenPreviousCardIsInteractive()
    {
        _player.PlayerEvents.Add(new CardExecutedEvent
        {
            GameId = _game.GameId,
            HappenedAt = DateTime.UtcNow,
            Round = 1,
            Username = _player.Username,
            Card = ProgrammingCard.MovementChoice
        });

        var exception = Assert.Throws<CustomException>(() =>
            _action.Execute(_player, _game, _systemTimeMock.Object, null));

        Assert.Contains("Cannot repeat interactive card", exception.Message);
        Assert.Equal(400, exception.StatusCode);
    }

    [Fact]
    public void Execute_ShouldRepeatLastCard()
    {
        _player.CurrentPosition = new Position(5, 5);
        _player.CurrentFacingDirection = Direction.North;

        _player.PlayerEvents.Add(new CardExecutedEvent
        {
            GameId = _game.GameId,
            HappenedAt = DateTime.UtcNow,
            Round = 1,
            Username = _player.Username,
            Card = ProgrammingCard.Move2
        });

        _action.Execute(_player, _game, _systemTimeMock.Object, null);

        Assert.Equal(new Position(5, 3), _player.CurrentPosition);
        Assert.Equal(Direction.North, _player.CurrentFacingDirection);

        var lastExecutedCard = _player.GetLastExecutedCard();
        Assert.Equal(ProgrammingCard.Move2, lastExecutedCard);   
    }
}
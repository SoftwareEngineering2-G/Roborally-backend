using Moq;
using Roborally.core.domain;
using Roborally.core.domain.Bases;
using Roborally.core.domain.Game;
using Roborally.core.domain.Game.CardActions;
using Roborally.core.domain.Game.Player;
using Roborally.unitTests.Factory;

namespace Roborally.unitTests.Domain.ActionCardTest;

public class SwapPositionCardActionTests
{
    private readonly Game _game;
    private readonly Player _player;
    private readonly Player _targetPlayer;
    private readonly SwapPositionCardAction _action;
    private readonly Mock<ISystemTime> _systemTimeMock;

    public SwapPositionCardActionTests()
    {
        _game = GameFactory.GetValidGame();
        _player = _game.Players[0];
        _targetPlayer = _game.Players[1];
        _action = new SwapPositionCardAction();
        _systemTimeMock = new Mock<ISystemTime>();
        _systemTimeMock.Setup(x => x.CurrentTime).Returns(DateTime.UtcNow);
    }

    [Fact]
    public void Execute_ShouldSwapPlayerPositions()
    {
        _player.CurrentPosition = new Position(2, 2);
        _targetPlayer.CurrentPosition = new Position(7, 3);

        var context = new CardExecutionContext
        {
            TargetPlayerUsername = _targetPlayer.Username
        };

        _action.Execute(_player, _game, _systemTimeMock.Object, context);

        Assert.Equal(new Position(7, 3), _player.CurrentPosition);
        Assert.Equal(new Position(2, 2), _targetPlayer.CurrentPosition);
    }

    [Fact]
    public void Execute_ShouldThrow_WhenTargetNotProvided()
    {
        var exception = Assert.Throws<CustomException>(() =>
            _action.Execute(_player, _game, _systemTimeMock.Object, null));

        Assert.Contains("requires a target player", exception.Message);
        Assert.Equal(400, exception.StatusCode);
    }

    [Fact]
    public void Execute_ShouldThrow_WhenTargetDoesNotExist()
    {
        var context = new CardExecutionContext
        {
            TargetPlayerUsername = "NonExisting"
        };

        var exception = Assert.Throws<CustomException>(() =>
            _action.Execute(_player, _game, _systemTimeMock.Object, context));

        Assert.Contains("does not exist", exception.Message);
        Assert.Equal(404, exception.StatusCode);
    }
}

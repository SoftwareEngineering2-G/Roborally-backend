using Moq;
using Roborally.core.domain;
using Roborally.core.domain.Bases;
using Roborally.core.domain.Game;
using Roborally.core.domain.Game.CardActions;
using Roborally.core.domain.Game.Deck;
using Roborally.core.domain.Game.Player;
using Roborally.unitTests.Factory;

namespace Roborally.unitTests.Domain.ActionCardTest;

public class MovementChoiceCardActionTests
{
    private readonly Game _game;
    private readonly Player _player;
    private readonly MovementChoiceCardAction _action;
    private readonly Mock<ISystemTime> _systemTimeMock;

    public MovementChoiceCardActionTests()
    {
        _game = GameFactory.GetValidGame();
        _player = _game.Players[0];
        _action = new MovementChoiceCardAction();
        _systemTimeMock = new Mock<ISystemTime>();
        _systemTimeMock.Setup(x => x.CurrentTime).Returns(DateTime.UtcNow);
    }

    [Fact]
    public void Execute_ShouldMovePlayerBySelectedCard()
    {
        _player.CurrentPosition = new Position(4, 4);
        _player.CurrentFacingDirection = Direction.North;

        var context = new CardExecutionContext
        {
            SelectedMovementCard = ProgrammingCard.Move2
        };

        _action.Execute(_player, _game, _systemTimeMock.Object, context);

        Assert.Equal(new Position(4, 2), _player.CurrentPosition);
        Assert.Equal(ProgrammingCard.MovementChoice, _player.GetLastExecutedCard());
    }

    [Fact]
    public void Execute_ShouldMoveBackward_WhenMoveBackSelected()
    {
        _player.CurrentPosition = new Position(6, 6);
        _player.CurrentFacingDirection = Direction.North;

        var context = new CardExecutionContext
        {
            SelectedMovementCard = ProgrammingCard.MoveBack
        };

        _action.Execute(_player, _game, _systemTimeMock.Object, context);

        Assert.Equal(new Position(6, 7), _player.CurrentPosition);
    }

    [Fact]
    public void Execute_ShouldRotate_WhenRotateOptionSelected()
    {
        _player.CurrentFacingDirection = Direction.North;

        var context = new CardExecutionContext
        {
            SelectedMovementCard = ProgrammingCard.RotateRight
        };

        _action.Execute(_player, _game, _systemTimeMock.Object, context);

        Assert.Equal(Direction.East, _player.CurrentFacingDirection);
    }

    [Fact]
    public void Execute_ShouldThrow_WhenSelectionMissing()
    {
        var exception = Assert.Throws<CustomException>(() =>
            _action.Execute(_player, _game, _systemTimeMock.Object, null));

        Assert.Contains("requires a selected movement option", exception.Message);
        Assert.Equal(400, exception.StatusCode);
    }
}

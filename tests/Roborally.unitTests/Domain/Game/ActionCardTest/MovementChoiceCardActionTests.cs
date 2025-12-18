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
    private readonly core.domain.Game.Game _game;
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

    [Fact]
    public void Execute_ShouldThrow_WhenCardNotAllowed()
    {
        var context = new CardExecutionContext
        {
            SelectedMovementCard = ProgrammingCard.Again
        };
        
        var exception = Assert.Throws<CustomException>(() =>
            _action.Execute(_player, _game, _systemTimeMock.Object, context));
        
        Assert.Contains("is not a valid movement option", exception.Message);
        Assert.Equal(400, exception.StatusCode);
    }
    
    [Fact]
    public void Execute_Move1()
    {
        _player.CurrentPosition = new Position(4, 4);
        _player.CurrentFacingDirection = Direction.North;
        
        var context = new CardExecutionContext
        {
            SelectedMovementCard = ProgrammingCard.Move1
        };
        
        _action.Execute(_player, _game, _systemTimeMock.Object, context);
        
        Assert.Equal(new Position(4, 3), _player.CurrentPosition);
    }
    
    [Fact]
    public void Execute_Move2()
    {
        _player.CurrentPosition = new Position(4, 4);
        _player.CurrentFacingDirection = Direction.North;
        
        var context = new CardExecutionContext
        {
            SelectedMovementCard = ProgrammingCard.Move2
        };
        
        _action.Execute(_player, _game, _systemTimeMock.Object, context);
        
        Assert.Equal(new Position(4, 2), _player.CurrentPosition);
    }
    
    [Fact]
    public void Execute_Move3()
    {
        _player.CurrentPosition = new Position(4, 4);
        _player.CurrentFacingDirection = Direction.North;
        
        var context = new CardExecutionContext
        {
            SelectedMovementCard = ProgrammingCard.Move3
        };
        
        _action.Execute(_player, _game, _systemTimeMock.Object, context);
        
        Assert.Equal(new Position(4, 1), _player.CurrentPosition);
    }

    [Fact]
    public void Execute_MoveBack()
    {
        _player.CurrentPosition = new Position(4, 4);
        _player.CurrentFacingDirection = Direction.North;
        
        var context = new CardExecutionContext
        {
            SelectedMovementCard = ProgrammingCard.MoveBack
        };
        
        _action.Execute(_player, _game, _systemTimeMock.Object, context);
        
        Assert.Equal(new Position(4, 5), _player.CurrentPosition);
    }
    
    [Fact]
    public void Execute_RotateLeft()
    {
        _player.CurrentFacingDirection = Direction.North;
        
        var context = new CardExecutionContext
        {
            SelectedMovementCard = ProgrammingCard.RotateLeft
        };
        
        _action.Execute(_player, _game, _systemTimeMock.Object, context);
        
        Assert.Equal(Direction.West, _player.CurrentFacingDirection);   
    }
    
    [Fact]
    public void Execute_RotateRight()
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
    public void Execute_UTurn()
    {
        _player.CurrentFacingDirection = Direction.North;
        
        var context = new CardExecutionContext
        {
            SelectedMovementCard = ProgrammingCard.UTurn
        };
        
        _action.Execute(_player, _game, _systemTimeMock.Object, context);
        
        Assert.Equal(Direction.South, _player.CurrentFacingDirection);
    }
}

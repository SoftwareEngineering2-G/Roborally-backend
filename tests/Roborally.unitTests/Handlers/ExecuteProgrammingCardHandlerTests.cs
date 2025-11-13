using Moq;
using Roborally.core.application;
using Roborally.core.application.ApplicationContracts.Broadcasters;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.CommandContracts.Game;
using Roborally.core.application.CommandHandlers.Game;
using Roborally.core.domain;
using Roborally.core.domain.Bases;
using Roborally.core.domain.Game;
using Roborally.core.domain.Game.Player;
using Roborally.unitTests.Factory;

namespace Roborally.unitTests.Handlers;

public class ExecuteProgrammingCardHandlerTests
{
    private readonly Mock<IGameRepository> _gameRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IGameBroadcaster> _gameBroadcasterMock;
    private readonly Mock<ISystemTime> _systemTimeMock;
    private readonly ExecuteProgrammingCardCommandHandler _handler;

    public ExecuteProgrammingCardHandlerTests()
    {
        _gameRepositoryMock = new Mock<IGameRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _gameBroadcasterMock = new Mock<IGameBroadcaster>();
        _systemTimeMock = new Mock<ISystemTime>();
        
        _systemTimeMock.Setup(x => x.CurrentTime).Returns(DateTime.UtcNow);
        
        _handler = new ExecuteProgrammingCardCommandHandler(
            _gameRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _gameBroadcasterMock.Object,
            _systemTimeMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowException_WhenGameNotFound()
    {
        // Arrange
        var command = new ExecuteProgrammingCardCommand
        {
            GameId = Guid.NewGuid(),
            Username = "player1",
            CardName = "Move1"
        };

        _gameRepositoryMock.Setup(r => r.FindAsync(command.GameId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Game?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CustomException>(() => 
            _handler.ExecuteAsync(command, CancellationToken.None));
        
        Assert.Equal("Game not found", exception.Message);
        Assert.Equal(404, exception.StatusCode);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowException_WhenInvalidCardName()
    {
        // Arrange
        var game = GameFactory.GetValidGame();
        game.CurrentPhase = GamePhase.ActivationPhase; // Set to activation phase
        var player = game.Players[0];
        var command = new ExecuteProgrammingCardCommand
        {
            GameId = game.GameId,
            Username = player.Username,
            CardName = "InvalidCard"
        };

        _gameRepositoryMock.Setup(r => r.FindAsync(command.GameId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(game);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CustomException>(() => 
            _handler.ExecuteAsync(command, CancellationToken.None));
        
        Assert.Contains("Invalid card name", exception.Message);
        Assert.Equal(400, exception.StatusCode);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldExecuteMove1Card_AndUpdatePlayerPosition()
    {
        // Arrange
        var game = GameFactory.GetValidGame();
        game.CurrentPhase = GamePhase.ActivationPhase; // Set to activation phase
        var player = game.Players[0];
        player.CurrentPosition = new Position(5, 5);
        player.CurrentFacingDirection = Direction.North;

        var command = new ExecuteProgrammingCardCommand
        {
            GameId = game.GameId,
            Username = player.Username,
            CardName = "Move 1"
        };

        _gameRepositoryMock.Setup(r => r.FindAsync(command.GameId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(game);

        // Act
        var response = await _handler.ExecuteAsync(command, CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(5, response.PlayerState.PositionX);
        Assert.Equal(4, response.PlayerState.PositionY); // Moved north (Y-1)
        Assert.Equal("North", response.PlayerState.Direction);
        Assert.Contains("Move 1", response.Message);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldExecuteRotateRightCard_AndUpdatePlayerDirection()
    {
        // Arrange
        var game = GameFactory.GetValidGame();
        game.CurrentPhase = GamePhase.ActivationPhase; // Set to activation phase
        var player = game.Players[0];
        player.CurrentPosition = new Position(5, 5);
        player.CurrentFacingDirection = Direction.North;

        var command = new ExecuteProgrammingCardCommand
        {
            GameId = game.GameId,
            Username = player.Username,
            CardName = "Rotate Right"
        };

        _gameRepositoryMock.Setup(r => r.FindAsync(command.GameId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(game);

        // Act
        var response = await _handler.ExecuteAsync(command, CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(5, response.PlayerState.PositionX); // Position unchanged
        Assert.Equal(5, response.PlayerState.PositionY); // Position unchanged
        Assert.Equal("East", response.PlayerState.Direction); // Rotated right from North to East
        Assert.Contains("Rotate Right", response.Message);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldExecuteRotateLeftCard_AndUpdatePlayerDirection()
    {
        // Arrange
        var game = GameFactory.GetValidGame();
        game.CurrentPhase = GamePhase.ActivationPhase; // Set to activation phase
        var player = game.Players[0];
        player.CurrentPosition = new Position(5, 5);
        player.CurrentFacingDirection = Direction.North;

        var command = new ExecuteProgrammingCardCommand
        {
            GameId = game.GameId,
            Username = player.Username,
            CardName = "Rotate Left"
        };

        _gameRepositoryMock.Setup(r => r.FindAsync(command.GameId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(game);

        // Act
        var response = await _handler.ExecuteAsync(command, CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(5, response.PlayerState.PositionX); // Position unchanged
        Assert.Equal(5, response.PlayerState.PositionY); // Position unchanged
        Assert.Equal("West", response.PlayerState.Direction); // Rotated left from North to West
        Assert.Contains("Rotate Left", response.Message);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldExecuteUTurnCard_AndUpdatePlayerDirection()
    {
        // Arrange
        var game = GameFactory.GetValidGame();
        game.CurrentPhase = GamePhase.ActivationPhase; // Set to activation phase
        var player = game.Players[0];
        player.CurrentPosition = new Position(5, 5);
        player.CurrentFacingDirection = Direction.North;

        var command = new ExecuteProgrammingCardCommand
        {
            GameId = game.GameId,
            Username = player.Username,
            CardName = "U-Turn"
        };

        _gameRepositoryMock.Setup(r => r.FindAsync(command.GameId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(game);

        // Act
        var response = await _handler.ExecuteAsync(command, CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(5, response.PlayerState.PositionX); // Position unchanged
        Assert.Equal(5, response.PlayerState.PositionY); // Position unchanged
        Assert.Equal("South", response.PlayerState.Direction); // U-turn from North to South
        Assert.Contains("U-Turn", response.Message);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldExecuteMove3Card_AndMovePlayerThreeSpaces()
    {
        // Arrange
        var game = GameFactory.GetValidGame();
        game.CurrentPhase = GamePhase.ActivationPhase; // Set to activation phase
        var player = game.Players[0];
        player.CurrentPosition = new Position(5, 5);
        player.CurrentFacingDirection = Direction.South;

        var command = new ExecuteProgrammingCardCommand
        {
            GameId = game.GameId,
            Username = player.Username,
            CardName = "Move 3"
        };

        _gameRepositoryMock.Setup(r => r.FindAsync(command.GameId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(game);

        // Act
        var response = await _handler.ExecuteAsync(command, CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(5, response.PlayerState.PositionX);
        Assert.Equal(8, response.PlayerState.PositionY); // Moved 3 spaces south
        Assert.Equal("South", response.PlayerState.Direction);
        Assert.Contains("Move 3", response.Message);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldExecuteMoveBackCard_AndMovePlayerBackward()
    {
        // Arrange
        var game = GameFactory.GetValidGame();
        game.CurrentPhase = GamePhase.ActivationPhase; // Set to activation phase
        var player = game.Players[0];
        player.CurrentPosition = new Position(5, 5);
        player.CurrentFacingDirection = Direction.North;

        var command = new ExecuteProgrammingCardCommand
        {
            GameId = game.GameId,
            Username = player.Username,
            CardName = "Move Back"
        };

        _gameRepositoryMock.Setup(r => r.FindAsync(command.GameId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(game);

        // Act
        var response = await _handler.ExecuteAsync(command, CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(5, response.PlayerState.PositionX);
        Assert.Equal(6, response.PlayerState.PositionY); // Moved backward (south) from facing north
        Assert.Equal("North", response.PlayerState.Direction); // Direction stays the same
        Assert.Contains("Move Back", response.Message);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldSaveChanges_AfterCardExecution()
    {
        // Arrange
        var game = GameFactory.GetValidGame();
        game.CurrentPhase = GamePhase.ActivationPhase; // Set to activation phase
        var player = game.Players[0];
        player.CurrentPosition = new Position(5, 5);
        player.CurrentFacingDirection = Direction.North;

        var command = new ExecuteProgrammingCardCommand
        {
            GameId = game.GameId,
            Username = player.Username,
            CardName = "Move 1"
        };

        _gameRepositoryMock.Setup(r => r.FindAsync(command.GameId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(game);

        // Act
        await _handler.ExecuteAsync(command, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldBroadcastRobotMovement_AfterCardExecution()
    {
        // Arrange
        var game = GameFactory.GetValidGame();
        game.CurrentPhase = GamePhase.ActivationPhase; // Set to activation phase
        var player = game.Players[0];
        player.CurrentPosition = new Position(5, 5);
        player.CurrentFacingDirection = Direction.North;

        var command = new ExecuteProgrammingCardCommand
        {
            GameId = game.GameId,
            Username = player.Username,
            CardName = "Move 1"
        };

        _gameRepositoryMock.Setup(r => r.FindAsync(command.GameId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(game);

        // Act
        await _handler.ExecuteAsync(command, CancellationToken.None);

        // Assert
        _gameBroadcasterMock.Verify(b => b.BroadcastRobotMovedAsync(
            command.GameId,
            player.Username,
            5, // X position
            4, // Y position (moved north)
            "North",
            "Move 1",
            It.IsAny<CancellationToken>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldRecordCardExecution_InPlayer()
    {
        // Arrange
        var game = GameFactory.GetValidGame();
        game.CurrentPhase = GamePhase.ActivationPhase; // Set to activation phase
        var player = game.Players[0];
        player.CurrentPosition = new Position(5, 5);
        player.CurrentFacingDirection = Direction.North;

        var command = new ExecuteProgrammingCardCommand
        {
            GameId = game.GameId,
            Username = player.Username,
            CardName = "Move 1"
        };

        _gameRepositoryMock.Setup(r => r.FindAsync(command.GameId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(game);

        // Act
        await _handler.ExecuteAsync(command, CancellationToken.None);

        // Assert
        var lastCard = player.GetLastExecutedCard();
        Assert.NotNull(lastCard);
        Assert.Equal("Move 1", lastCard.DisplayName);
    }
    
}

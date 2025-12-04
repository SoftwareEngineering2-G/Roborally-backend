using Moq;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.CommandContracts.Game;
using Roborally.core.application.CommandHandlers.Game;
using Roborally.core.domain;
using Roborally.core.domain.Game;
using Roborally.unitTests.Factory;

namespace Roborally.unitTests.CommandHandlers.Game;

public class GetCurrentGameStateHandlerTests
{
    private readonly Mock<IGameRepository> _gameRepositoryMock;
    private readonly GetCurrentGameStateCommandHandler _handler;

    public GetCurrentGameStateHandlerTests()
    {
        _gameRepositoryMock = new Mock<IGameRepository>();
        _handler = new GetCurrentGameStateCommandHandler(_gameRepositoryMock.Object);
    }

    [Fact]
    public async Task CannotGetCurrentGameState_WhenGameDoesNotExist()
    {
        // Arrange
        _gameRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((core.domain.Game.Game?)null);

        var command = new GetCurrentGameStateCommand
        {
            GameId = Guid.NewGuid(),
            Username = "SomePlayer"
        };

        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => _handler.ExecuteAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task GetCurrentGameState_WhenGameExists_InActivationPhase()
    {
        // Arrange
        var game = GameFactory.GetValidGame();
        game.CurrentPhase = GamePhase.ActivationPhase;
        _gameRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(game);

        var command = new GetCurrentGameStateCommand()
        {
            GameId = game.GameId,
            Username = game.Players[0].Username
        };

        // Act
        var getCurrentGameStateResponse = await _handler.ExecuteAsync(command, CancellationToken.None);

        // Assert
        Assert.NotNull(getCurrentGameStateResponse);
        Assert.Equal(getCurrentGameStateResponse.PersonalState,
            new GetCurrentGameStateCommandResponse.MyState(null, null, 0, 0));
    }

    [Fact]
    public async Task GetCurrentGameState_WhenGameExists_NotInActivationPhase()
    {
        // Arrange
        var game = GameFactory.GetValidGame();
        game.CurrentPhase = GamePhase.ProgrammingPhase;
        _gameRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(game);

        var command = new GetCurrentGameStateCommand()
        {
            GameId = game.GameId,
            Username = game.Players[0].Username
        };
        
        // Act
        var getCurrentGameStateResponse = await _handler.ExecuteAsync(command, CancellationToken.None);
        
        // Assert
        Assert.NotNull(getCurrentGameStateResponse);
    }
}
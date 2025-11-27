using Moq;
using Roborally.core.application.ApplicationContracts.Broadcasters;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.CommandContracts.Game;
using Roborally.core.application.CommandHandlers.Game;
using Roborally.core.domain;
using Roborally.core.domain.Bases;
using Roborally.core.domain.Game;
using Roborally.core.domain.Game.Deck;
using Roborally.unitTests.Factory;

namespace Roborally.unitTests.CommandHandlers.Game;

public class RevealNextRegisterHandlerTests
{
    private readonly Mock<IGameRepository> _gameRepository;
    private readonly Mock<IGameBroadcaster> _gameBroadcaster;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly RevealNextRegisterCommandHandler _handler;

    public RevealNextRegisterHandlerTests()
    {
        _gameRepository = new Mock<IGameRepository>();
        _gameBroadcaster = new Mock<IGameBroadcaster>();
        _unitOfWork = new Mock<IUnitOfWork>();

        _handler = new RevealNextRegisterCommandHandler(_gameRepository.Object, _gameBroadcaster.Object,
            _unitOfWork.Object);
    }

    [Fact]
    public async Task CannotRevealNextRegister_WhenGameDoesNotExist()
    {
        // Arrange
        _gameRepository.Setup(repo => repo.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((core.domain.Game.Game?)null);

        var command = new RevealNextRegisterCommand
        {
            GameId = Guid.NewGuid(),
            Username = "SomePlayer"
        };
        
        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => _handler.ExecuteAsync(command, CancellationToken.None));
    }
    
    [Fact]
    public async Task CannotRevealNextRegister_WhenUserIsNotHost()
    {
        // Arrange
        var game = GameFactory.GetValidGame();
        game.HostUsername = game.Players[1].Username;
        _gameRepository.Setup(repo => repo.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(game);

        var command = new RevealNextRegisterCommand
        {
            GameId = Guid.NewGuid(),
            Username = game.Players[0].Username
        };
        
        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => _handler.ExecuteAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task RevealNextRegister_WhenGameExists_AndUserIsHost()
    {
        // Arrange
        var game = GameFactory.GetValidGame();
        game.HostUsername = game.Players[0].Username;
        game.CurrentPhase = GamePhase.ActivationPhase;
        _gameRepository.Setup(repo => repo.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(game);

        var command = new RevealNextRegisterCommand
        {
            GameId = game.GameId,
            Username = game.Players[0].Username
        };
        
        // Act
        await _handler.ExecuteAsync(command, CancellationToken.None);
        
        // Assert
        _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _gameBroadcaster.Verify(broadcaster =>
            broadcaster.BroadcastRegisterRevealedAsync(
                command.GameId,
                game.CurrentRevealedRegister,
                It.IsAny<Dictionary<string, ProgrammingCard>>(),
                It.IsAny<CancellationToken>()), Times.Once);
        _gameBroadcaster.Verify(broadcaster =>
            broadcaster.BroadcastNextPlayerInTurn(
                command.GameId,
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()), Times.Once);
    }
}
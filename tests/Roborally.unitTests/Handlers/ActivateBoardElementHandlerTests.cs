// using Moq;
// using Roborally.core.application.CommandContracts.Game;
// using Roborally.core.application.CommandHandlers.Game;
// using Roborally.core.domain;
// using Roborally.core.domain.Bases;
// using Roborally.core.domain.Game;
// using Roborally.core.domain.Game.Gameboard.BoardElement;
// using Roborally.core.domain.Game.Gameboard.Spaces;
// using Roborally.core.domain.Game.Player;
// using Roborally.unitTests.Factory;
//
// namespace Roborally.unitTests.Handlers;
//
// public class ActivateBoardElementHandlerTests
// {
//     private readonly Mock<IGameRepository> _gameRepositoryMock;
//     private readonly Mock<IUnitOfWork> _unitOfWorkMock;
//     private readonly ActivateBoardElementCommandHandler handler;
//
//     public ActivateBoardElementHandlerTests()
//     {
//         _gameRepositoryMock = new Mock<IGameRepository>();
//         _unitOfWorkMock = new Mock<IUnitOfWork>();
//
//         handler = new ActivateBoardElementCommandHandler(_unitOfWorkMock.Object, _gameRepositoryMock.Object);
//     }
//
//     [Fact]
//     public void CannotActivateBoardElement_WhenGameDoesNotExist()
//     {
//         // Arrange
//         var gameRepositoryMock = new Mock<IGameRepository>();
//         gameRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
//             .ReturnsAsync((Game?)null);
//
//         var command = new ActivateBoardElementCommand
//         {
//             GameId = Guid.NewGuid(),
//             Username = "Player1"
//         };
//
//         // Act & Assert
//         var exception =
//             Assert.ThrowsAsync<CustomException>(() => handler.ExecuteAsync(command, CancellationToken.None));
//     }
//
//     [Fact]
//     public void CannotActivateBoardElement_WhenPlayerDoesNotExist()
//     {
//         // Arrange
//         var game = GameFactory.GetValidGame();
//         _gameRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
//             .ReturnsAsync(game);
//         var command = new ActivateBoardElementCommand
//         {
//             GameId = game.GameId,
//             Username = "NonExistentPlayer"
//         };
//
//         // Act & Assert
//         var exception =
//             Assert.ThrowsAsync<CustomException>(() => handler.ExecuteAsync(command, CancellationToken.None));
//     }
//
//     [Fact]
//     public async Task ActivateBoardElement_NoOp_WhenNoBoardElementAtPlayerPosition()
//     {
//         // Arrange
//         var game = GameFactory.GetValidGame();
//         var player = game.Players[0];
//         player.CurrentPosition = new Position(0, 0);
//         game.GameBoard.Spaces[0][0] = new EmptySpace(); // No board element at player's position
//
//         _gameRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
//             .ReturnsAsync(game);
//
//         var command = new ActivateBoardElementCommand
//         {
//             GameId = game.GameId,
//             Username = player.Username
//         };
//
//         // Act
//         await handler.ExecuteAsync(command, CancellationToken.None);
//
//         // Assert
//         _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
//     }
//
//     [Fact]
//     public async Task ActivateBoardElement_Succeeds_WhenBoardElementExists()
//     {
//         // Arrange
//         var game = GameFactory.GetValidGame();
//         var player = game.Players[0];
//         player.CurrentPosition = new Position(0, 0);
//         var initialDirection = player.CurrentFacingDirection;
//         game.GameBoard.Spaces[0][0] = new Gear() { Direction = GearDirection.ClockWise };
//
//         _gameRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
//             .ReturnsAsync(game);
//
//         var command = new ActivateBoardElementCommand
//         {
//             GameId = game.GameId,
//             Username = player.Username
//         };
//
//         // Act
//         await handler.ExecuteAsync(command, CancellationToken.None);
//
//         // Assert
//         Assert.Equal(player.CurrentFacingDirection, initialDirection.RotateRight());
//         _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
//     }
// }
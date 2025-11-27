using Moq;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.CommandContracts;
using Roborally.core.application.CommandHandlers.GameLobby;
using Roborally.unitTests.Factory;

namespace Roborally.unitTests.CommandHandlers.GameLobby
{
    public class GetUserPausedgamesHandlerTests
    {
        private readonly Mock<IGameRepository> _gameRepositoryMock;
        private readonly GetUserPausedGamesCommandHandler _handler;

        public GetUserPausedgamesHandlerTests()
        {
            _gameRepositoryMock = new Mock<IGameRepository>();
            _handler = new GetUserPausedGamesCommandHandler(_gameRepositoryMock.Object);
        }

        [Fact]
        public async Task ReturnsEmptyList_WhenNoPausedGames()
        {
            // Arrange
            _gameRepositoryMock
                .Setup(repo => repo.FindPausedGamesForUserAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync([]);

            var command = new GetUserPausedGamesCommand
            {
                Username = "someuser"
            };

            // Act
            var result = await _handler.ExecuteAsync(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _gameRepositoryMock.Verify(repo => repo.FindPausedGamesForUserAsync(command.Username, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ReturnsMappedGames_WhenRepositoryReturnsPausedGames()
        {
            // Arrange
            var game = GameFactory.GetValidGame();
            game.IsPaused = true;
            // ensure players present
            var playerUsernames = game.Players.Select(p => p.Username).ToArray();

            _gameRepositoryMock
                .Setup(repo => repo.FindPausedGamesForUserAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync([game]);

            var command = new GetUserPausedGamesCommand
            {
                Username = playerUsernames.First()
            };

            // Act
            var result = await _handler.ExecuteAsync(command, CancellationToken.None);

            // Assert
            Assert.Single(result);
            var mapped = result.First();
            Assert.Equal(game.GameId, mapped.GameId);
            Assert.Equal(game.HostUsername, mapped.HostUsername);
            Assert.Equal(game.Name, mapped.Name);
            Assert.Equal(playerUsernames, mapped.PlayerUsernames);
            _gameRepositoryMock.Verify(repo => repo.FindPausedGamesForUserAsync(command.Username, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

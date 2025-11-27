using Moq;
using Roborally.core.application.ApplicationContracts.Broadcasters;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.CommandContracts;
using Roborally.core.application.CommandHandlers.GameLobby;
using Roborally.core.domain;
using Roborally.core.domain.Bases;
using Roborally.core.domain.Game.Gameboard;
using Roborally.unitTests.Factory;

namespace Roborally.unitTests.CommandHandlers.GameLobby
{
    public class StartGameHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IGameLobbyRepository> _gameLobbyRepositoryMock;
        private readonly Mock<IGameRepository> _gameRepositoryMock;
        private readonly Mock<IGameBoardRepository> _gameBoardRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ISystemTime> _systemTimeMock;
        private readonly Mock<IGameLobbyBroadcaster> _gameLobbyBroadcasterMock;
        private readonly StartGameCommandHandler _handler;

        public StartGameHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _gameLobbyRepositoryMock = new Mock<IGameLobbyRepository>();
            _gameRepositoryMock = new Mock<IGameRepository>();
            _gameBoardRepositoryMock = new Mock<IGameBoardRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _systemTimeMock = new Mock<ISystemTime>();
            _gameLobbyBroadcasterMock = new Mock<IGameLobbyBroadcaster>();

            _handler = new StartGameCommandHandler(
                _userRepositoryMock.Object,
                _gameLobbyRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _systemTimeMock.Object,
                _gameLobbyBroadcasterMock.Object,
                _gameRepositoryMock.Object,
                _gameBoardRepositoryMock.Object
            );
        }

        [Fact]
        public async Task CannotStartGame_WhenUserDoesNotExist()
        {
            // Arrange
            _userRepositoryMock.Setup(repo => repo.ExistsByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var command = new StartGameCommand
            {
                GameId = Guid.NewGuid(),
                Username = "NonExistentUser",
                GameBoardName = "AnyBoard"
            };

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(() => _handler.ExecuteAsync(command, CancellationToken.None));
        }

        [Fact]
        public async Task CannotStartGame_WhenLobbyDoesNotExist()
        {
            // Arrange
            _userRepositoryMock.Setup(repo => repo.ExistsByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _gameLobbyRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Roborally.core.domain.Lobby.GameLobby?)null);

            var command = new StartGameCommand
            {
                GameId = Guid.NewGuid(),
                Username = "ValidUser",
                GameBoardName = "AnyBoard"
            };

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(() => _handler.ExecuteAsync(command, CancellationToken.None));
        }

        [Fact]
        public async Task CannotStartGame_WhenGameBoardDoesNotExist()
        {
            // Arrange
            _userRepositoryMock.Setup(repo => repo.ExistsByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var hostUser = UserFactory.GetValidUser();
            var lobby = GameLobbyFactory.GetValidGameLobby(hostUser, _systemTimeMock.Object);

            _gameLobbyRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>()))
                .ReturnsAsync(lobby);

            _gameBoardRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((GameBoard?)null);

            var command = new StartGameCommand
            {
                GameId = lobby.GameId,
                Username = hostUser.Username,
                GameBoardName = "NonExistentBoard"
            };

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(() => _handler.ExecuteAsync(command, CancellationToken.None));
        }

        [Fact]
        public async Task CanStartGame_WhenAllValid()
        {
            // Arrange
            _userRepositoryMock.Setup(repo => repo.ExistsByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var hostUser = UserFactory.GetValidUser();
            var otherUser = UserFactory.GetValidUser();
            var lobby = GameLobbyFactory.GetValidGameLobby(hostUser, _systemTimeMock.Object);
            lobby.JoinLobby(otherUser);
            
            _gameLobbyRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>()))
                .ReturnsAsync(lobby);

            _gameBoardRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(GameBoardFactory.GetStarterCourse());

            var command = new StartGameCommand
            {
                GameId = lobby.GameId,
                Username = hostUser.Username,
                GameBoardName = "ValidBoard"
            };

            // Act
            await _handler.ExecuteAsync(command, CancellationToken.None);

            // Assert
            _gameRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Roborally.core.domain.Game.Game>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _gameLobbyBroadcasterMock.Verify(broadcaster => broadcaster.BroadcastGameStartedAsync(command.GameId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

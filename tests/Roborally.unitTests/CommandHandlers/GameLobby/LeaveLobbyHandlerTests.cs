using Moq;
using Roborally.core.application.ApplicationContracts.Broadcasters;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.CommandContracts;
using Roborally.core.application.CommandHandlers.GameLobby;
using Roborally.core.domain;
using Roborally.core.domain.Bases;
using Roborally.unitTests.Factory;

namespace Roborally.unitTests.CommandHandlers.GameLobby;

public class LeaveLobbyHandlerTests
{
    private readonly Mock<IGameLobbyRepository> _gameLobbyRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IGameLobbyBroadcaster> _gameLobbyBroadcasterMock;
    private readonly LeaveLobbyCommandHandler _handler;

    public LeaveLobbyHandlerTests()
    {
        _gameLobbyRepositoryMock = new Mock<IGameLobbyRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _gameLobbyBroadcasterMock = new Mock<IGameLobbyBroadcaster>();

        _handler = new LeaveLobbyCommandHandler(_gameLobbyRepositoryMock.Object, _userRepositoryMock.Object,
            _unitOfWorkMock.Object, _gameLobbyBroadcasterMock.Object);
    }

    [Fact]
    public async Task CannotLeaveLobby_WhenUserDoesNotExist()
    {
        // Arrange
        _userRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Roborally.core.domain.User.User?)null);

        var command = new LeaveLobbyCommand()
        {
            GameId = Guid.NewGuid(),
            Username = "ValidUsername",
        };

        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => _handler.ExecuteAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task CannotLeaveLobby_WhenLobbyDoesNotExist()
    {
        // Arrange
        var user = UserFactory.GetValidUser();
        _userRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _gameLobbyRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Roborally.core.domain.Lobby.GameLobby?)null);

        var command = new LeaveLobbyCommand()
        {
            GameId = Guid.NewGuid(),
            Username = user.Username,
        };

        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => _handler.ExecuteAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task LeaveLobby_WhenOnlyOneUserLeft()
    {
        // Arrange
        var user = UserFactory.GetValidUser();
        var systemTimeMock = new Mock<ISystemTime>();
        var gameLobby = GameLobbyFactory.GetValidGameLobby(user, systemTimeMock.Object);

        _userRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _gameLobbyRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>()))
            .ReturnsAsync(gameLobby);

        var command = new LeaveLobbyCommand()
        {
            GameId = Guid.NewGuid(),
            Username = user.Username,
        };

        // Act
        await _handler.ExecuteAsync(command, CancellationToken.None);

        // Assert
        _gameLobbyRepositoryMock.Verify(repo => repo.Remove(It.IsAny<Roborally.core.domain.Lobby.GameLobby>()),
            Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _gameLobbyBroadcasterMock.Verify(
            broadcaster =>
                broadcaster.BroadcastUserLeftAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task LeaveLobby_WhenMultipleUsers_AndHostLeaves()
    {
        // Arrange
        var hostUser = UserFactory.GetValidUser();
        var otherUser = UserFactory.GetValidUser();
        var systemTimeMock = new Mock<ISystemTime>();
        var gameLobby = GameLobbyFactory.GetValidGameLobby(hostUser, systemTimeMock.Object);
        gameLobby.JoinLobby(otherUser);

        _userRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(hostUser);

        _gameLobbyRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>()))
            .ReturnsAsync(gameLobby);

        var command = new LeaveLobbyCommand()
        {
            GameId = Guid.NewGuid(),
            Username = hostUser.Username,
        };

        // Act
        await _handler.ExecuteAsync(command, CancellationToken.None);

        // Assert
        _gameLobbyRepositoryMock.Verify(repo => repo.Remove(It.IsAny<Roborally.core.domain.Lobby.GameLobby>()),
            Times.Never);
        _gameLobbyBroadcasterMock.Verify(
            broadcaster =>
                broadcaster.BroadcastHostChangedAsync(It.IsAny<Guid>(), It.IsAny<string>(),
                    It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _gameLobbyBroadcasterMock.Verify(
            broadcaster =>
                broadcaster.BroadcastUserLeftAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
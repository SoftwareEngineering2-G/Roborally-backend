using Moq;
using Roborally.core.application.ApplicationContracts.Broadcasters;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.CommandContracts;
using Roborally.core.application.CommandHandlers.GameLobby;
using Roborally.core.domain;
using Roborally.core.domain.Bases;
using Roborally.unitTests.Factory;

namespace Roborally.unitTests.CommandHandlers.GameLobby;

public class JoinLobbyHandlerTests
{
    private readonly Mock<IGameLobbyRepository> _gameLobbyRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IGameLobbyBroadcaster> _gameLobbyBroadcasterMock;
    private readonly JoinLobbyCommandHandler _handler;
    
    public JoinLobbyHandlerTests()
    {
        _gameLobbyRepositoryMock = new Mock<IGameLobbyRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _gameLobbyBroadcasterMock = new Mock<IGameLobbyBroadcaster>();
        
        _handler = new JoinLobbyCommandHandler(
            _gameLobbyRepositoryMock.Object,
            _userRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _gameLobbyBroadcasterMock.Object
        );
    }

    [Fact]
    public async Task CannotJoinLobby_WhenUserDoesNotExist()
    {
        // Arrange
        _userRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((core.domain.User.User?)null);

        var command = new JoinLobbyCommand()
        {
            GameId = Guid.NewGuid(),
            Username = "ValidUsername",
        };
        
        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => _handler.ExecuteAsync(command, CancellationToken.None));
    }
    
    [Fact]
    public async Task CannotJoinLobby_WhenLobbyDoesNotExist()
    {
        // Arrange
        var user = UserFactory.GetValidUser();
        _userRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        
        _gameLobbyRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>()))
            .ReturnsAsync((core.domain.Lobby.GameLobby?)null);

        var command = new JoinLobbyCommand()
        {
            GameId = Guid.NewGuid(),
            Username = "ValidUsername",
        };
        
        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => _handler.ExecuteAsync(command, CancellationToken.None));
    }
    
    [Fact]
    public async Task CanJoinLobby_WhenUserAndLobbyExist()
    {
        // Arrange
        var hostUser = UserFactory.GetValidUser();
        var newUser = UserFactory.GetValidUser();
        var systemTime = new Mock<ISystemTime>();
        var gameLobby = GameLobbyFactory.GetValidGameLobby(hostUser, systemTime.Object);
        
        _userRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(newUser);
        
        _gameLobbyRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Guid>()))
            .ReturnsAsync(gameLobby);

        var command = new JoinLobbyCommand()
        {
            GameId = Guid.NewGuid(),
            Username = newUser.Username,
        };
        
        // Act
        await _handler.ExecuteAsync(command, CancellationToken.None);
        
        // Assert
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _gameLobbyBroadcasterMock.Verify(broadcaster => 
            broadcaster.BroadcastUserJoinedAsync(command.GameId, command.Username, It.IsAny<CancellationToken>()), Times.Once);
    }
}
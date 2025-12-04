using Moq;
using Roborally.core.application;
using Roborally.core.domain;
using Roborally.core.domain.Bases;
using Roborally.core.domain.Game.Gameboard;
using Roborally.core.domain.Lobby;
using Roborally.unitTests.Factory;

namespace Roborally.unitTests.Domain;

public class GameLobbyTests
{
    private readonly Mock<ISystemTime> _systemTimeMock;

    public GameLobbyTests()
    {
        _systemTimeMock = new Mock<ISystemTime>();
        _systemTimeMock.Setup(x => x.CurrentTime).Returns(DateTime.UtcNow);
    }

    [Theory]
    [MemberData(nameof(GameLobbyFactory.GetValidGameLobbyRoomName), MemberType = typeof(GameLobbyFactory))]
    public void LobbyWith_ValidRoomName_CanBeCreated(string validRoomName)
    {
        // Arrange
        var hostUser = GameLobbyFactory.CreateValidUser();

        // Act
        var lobby = new GameLobby(hostUser, false, validRoomName, _systemTimeMock.Object);

        // Assert
        Assert.NotNull(lobby);
        Assert.Equal(hostUser.Username, lobby.HostUsername);
        Assert.Single(lobby.JoinedUsers); // host auto joined
        Assert.Equal(validRoomName.Trim(), lobby.Name);
        Assert.True(lobby.IsActive); // Newly created lobby is active
        Assert.Null(lobby.StartedAt); // Not started yet
        Assert.NotEqual(default, lobby.CreatedAt);
    }
    
    [Theory]
    [MemberData(nameof(GameLobbyFactory.GetInvalidGameRoomNames), MemberType = typeof(GameLobbyFactory))]
    public void Lobby_With_InvalidRoomName_CannotBeCreated(string invalidRoomName)
    {
        // Arrange
        var hostUser = GameLobbyFactory.CreateValidUser();

        // Act & Assert
        Assert.Throws<CustomException>(() => new GameLobby(hostUser, false, invalidRoomName, _systemTimeMock.Object));
    }
    
    [Theory]
    [MemberData(nameof(GameLobbyFactory.GetValidGameLobbyRoomName), MemberType = typeof(GameLobbyFactory))]
    public void UserWith_ValidRoomName_CanCreatePublicGame(string validRoomName)
    {
        // Arrange
        var hostUser = GameLobbyFactory.CreateValidUser();

        // Act
        var lobby = new GameLobby(hostUser, false, validRoomName, _systemTimeMock.Object);

        // Assert
        Assert.NotNull(lobby);
        Assert.Equal(hostUser.Username, lobby.HostUsername);
        Assert.False(lobby.IsPrivate); // public
        Assert.Equal(validRoomName.Trim(), lobby.Name);
        Assert.Single(lobby.JoinedUsers); // host auto joined
        Assert.True(lobby.IsActive); // Active lobby
    }
    
    [Theory]
    [MemberData(nameof(GameLobbyFactory.GetValidGameLobbyRoomName), MemberType = typeof(GameLobbyFactory))]
    public void UserWith_ValidRoomName_CanCreatePrivateGame(string validRoomName)
    {
        // Arrange
        var hostUser = GameLobbyFactory.CreateValidUser();

        // Act
        var lobby = new GameLobby(hostUser, true, validRoomName, _systemTimeMock.Object);

        // Assert
        Assert.NotNull(lobby);
        Assert.Equal(hostUser.Username, lobby.HostUsername);
        Assert.True(lobby.IsPrivate); // private
        Assert.Equal(validRoomName.Trim(), lobby.Name);
        Assert.Single(lobby.JoinedUsers); // host auto joined
        Assert.True(lobby.IsActive); // Active lobby
    }
    
    [Fact]
    public void AlreadyStartedGame_CannotBeJoined()
    {
        // Arrange
        var hostUser = GameLobbyFactory.CreateValidUser();
        var lobby = new GameLobby(hostUser, false, "Test Lobby", _systemTimeMock.Object)
        {
            StartedAt = DateTime.UtcNow
        };

        var newUser = GameLobbyFactory.CreateValidUser("newPlayer");

        // Act & Assert
        Assert.Throws<CustomException>(() => lobby.JoinLobby(newUser));
    }

    [Fact]
    public void FullLobby_CannotBeJoined()
    {
        // Arrange
        var hostUser = GameLobbyFactory.CreateValidUser();
        var lobby = new GameLobby(hostUser, false, "Test Lobby", _systemTimeMock.Object);

        // Fill the lobby to max capacity
        for (int i = 1; i < 6; i++)
        {
            var user = GameLobbyFactory.CreateValidUser($"player{i}");
            lobby.JoinLobby(user);
        }

        var newUser = GameLobbyFactory.CreateValidUser("newPlayer");

        // Act & Assert
        Assert.Throws<CustomException>(() => lobby.JoinLobby(newUser));
    }

    [Fact]
    public void AlreadyJoinedUser_CannotRejoinLobby()
    {
        // Arrange
        var hostUser = GameLobbyFactory.CreateValidUser();
        var lobby = new GameLobby(hostUser, false, "Test Lobby", _systemTimeMock.Object);

        // Act & Assert
        Assert.Throws<CustomException>(() => lobby.JoinLobby(hostUser));
    }

    [Fact]
    public void NotRequiredUser_CannotJoinLobby()
    {
        // Arrange
        var hostUser = GameLobbyFactory.CreateValidUser();
        var lobby = new GameLobby(hostUser, true, "Test Lobby", _systemTimeMock.Object);
        lobby.InitLobbyToContinue([hostUser]);
        
        var nonRequiredUser = GameLobbyFactory.CreateValidUser("nonRequiredPlayer");

        // Act & Assert
        Assert.Throws<CustomException>(() => lobby.JoinLobby(nonRequiredUser));
    }

    [Fact]
    public void WhenNotJoinedUserLeavesLobby_NothingHappens()
    {
        // Arrange
        var hostUser = GameLobbyFactory.CreateValidUser();
        var lobby = new GameLobby(hostUser, false, "Test Lobby", _systemTimeMock.Object);

        var nonJoinedUser = GameLobbyFactory.CreateValidUser("nonJoinedPlayer");

        // Act & Assert
        lobby.LeaveLobby(nonJoinedUser);
        Assert.Single(lobby.JoinedUsers); 
    }

    [Fact]
    public void NormalUser_CannotStartGame()
    {
        // Arrange
        var hostUser = GameLobbyFactory.CreateValidUser();
        var lobby = new GameLobby(hostUser, false, "Test Lobby", _systemTimeMock.Object);

        var normalUser = GameLobbyFactory.CreateValidUser("normalPlayer");
        lobby.JoinLobby(normalUser);

        // Act & Assert
        Assert.Throws<CustomException>(() => 
            lobby.StartGame(normalUser.Username, _systemTimeMock.Object, GameBoardFactory.GetEmptyBoard()));
    }

    [Fact]
    public void AlreadyStartedGame_CannotBeStared()
    {
        // Arrange
        var hostUser = GameLobbyFactory.CreateValidUser();
        var lobby = new GameLobby(hostUser, false, "Test Lobby", _systemTimeMock.Object)
        {
            StartedAt = DateTime.UtcNow
        };

        // Act & Assert
        Assert.Throws<CustomException>(() => 
            lobby.StartGame(hostUser.Username, _systemTimeMock.Object, GameBoardFactory.GetEmptyBoard()));
    }

    [Fact]
    public void NormalUser_CannotContinueGame()
    {
        // Arrange
        var hostUser = GameLobbyFactory.CreateValidUser();
        var lobby = new GameLobby(hostUser, false, "Test Lobby", _systemTimeMock.Object);

        var normalUser = GameLobbyFactory.CreateValidUser("normalPlayer");
        lobby.JoinLobby(normalUser);

        // Act & Assert
        Assert.Throws<CustomException>(() =>
            lobby.ContinueGame(normalUser.Username, _systemTimeMock.Object));
    }
    
    [Fact]
    public void AlreadyStartedGame_CannotBeContinued()
    {
        // Arrange
        var hostUser = GameLobbyFactory.CreateValidUser();
        var lobby = new GameLobby(hostUser, false, "Test Lobby", _systemTimeMock.Object)
        {
            StartedAt = DateTime.UtcNow
        };

        // Act & Assert
        Assert.Throws<CustomException>(() =>
            lobby.ContinueGame(hostUser.Username, _systemTimeMock.Object));
    }
}
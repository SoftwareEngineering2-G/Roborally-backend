using Moq;
using Roborally.core.application;
using Roborally.core.domain.Bases;
using Roborally.core.domain.Lobby;
using Roborally.unitTests.Factory;

namespace Roborally.unitTests.Domain;

public class GameLobbyTest
{
    private readonly Mock<ISystemTime> _systemTimeMock;

    public GameLobbyTest()
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
        Assert.Equal(validRoomName.Trim(), lobby.GameRoomName);
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
        Assert.Equal(validRoomName.Trim(), lobby.GameRoomName);
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
        Assert.Equal(validRoomName.Trim(), lobby.GameRoomName);
        Assert.Single(lobby.JoinedUsers); // host auto joined
        Assert.True(lobby.IsActive); // Active lobby
    }
    
}
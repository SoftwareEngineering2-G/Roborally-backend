using Roborally.core.application;
using Roborally.core.domain.Lobby;
using Roborally.unitTests.Factory;

namespace Roborally.unitTests.Domain;

public class GameLobbyTest
{
    [Theory]
    [MemberData(nameof(GameLobbyFactory.GetValidGameLobbyRoomName), MemberType = typeof(GameLobbyFactory))]
    public void LobbyWith_ValidRoomName_CanBeCreated(string validRoomName)
    {
        // Arrange
        var hostUser = GameLobbyFactory.CreateValidUser();

        // Act
        var lobby = new GameLobby(hostUser, false, validRoomName);

        // Assert
        Assert.NotNull(lobby);
        Assert.Equal(hostUser.Id, lobby.HostId);
        Assert.Single(lobby.GetUsersInLobby()); // host auto joined
        Assert.Equal(validRoomName.Trim(), lobby.GameRoomName);
    }
    
    [Theory]
    [MemberData(nameof(GameLobbyFactory.GetInvalidGameRoomNames), MemberType = typeof(GameLobbyFactory))]
    public void Lobby_With_InvalidRoomName_CannotBeCreated(string invalidRoomName)
    {
        // Arrange
        var hostUser = GameLobbyFactory.CreateValidUser();

        // Act & Assert
        Assert.Throws<CustomException>(() => new GameLobby(hostUser, false, invalidRoomName));
    }
    
    [Theory]
    [MemberData(nameof(GameLobbyFactory.GetValidGameLobbyRoomName), MemberType = typeof(GameLobbyFactory))]
    public void UserWith_ValidRoomName_CanCreatePublicGame(string validRoomName)
    {
        // Arrange
        var hostUser = GameLobbyFactory.CreateValidUser();

        // Act
        var lobby = new GameLobby(hostUser, false, validRoomName);

        // Assert
        Assert.NotNull(lobby);
        Assert.Equal(hostUser.Id, lobby.HostId);
        Assert.False(lobby.IsPrivate); // public
        Assert.Equal(validRoomName.Trim(), lobby.GameRoomName);
        Assert.Single(lobby.GetUsersInLobby()); // host auto joined
    }
    
    [Theory]
    [MemberData(nameof(GameLobbyFactory.GetValidGameLobbyRoomName), MemberType = typeof(GameLobbyFactory))]
    public void UserWith_ValidRoomName_CanCreatePrivateGame(string validRoomName)
    {
        // Arrange
        var hostUser = GameLobbyFactory.CreateValidUser();

        // Act
        var lobby = new GameLobby(hostUser, true, validRoomName);

        // Assert
        Assert.NotNull(lobby);
        Assert.Equal(hostUser.Id, lobby.HostId);
        Assert.True(lobby.IsPrivate); // private
        Assert.Equal(validRoomName.Trim(), lobby.GameRoomName);
        Assert.Single(lobby.GetUsersInLobby()); // host auto joined
    }
    
    
}
using Roborally.core.domain.Bases;
using Roborally.core.domain.Lobby;
using Roborally.core.domain.User;

namespace Roborally.unitTests.Factory;

public class GameLobbyFactory
{
    public static IEnumerable<object[]> GetValidGameLobbyRoomName()
    {
        return new List<object[]>()
        {
            new object[] { "Robot Championship" },
            new object[] { "Quick Match" },
            new object[] { "Special-Chars_Allowed!" },
            new object[] { "AA" },
        };
    }

    public static IEnumerable<object[]> GetInvalidGameRoomNames()
    {
        return new List<object[]>()
        {
            new object[] { " " },
            new object[] { "A" },
        };
    }

    public static IEnumerable<object[]> GetValidPrivacySettings()
    {
        return new List<object[]>()
        {
            new object[] { true },
            new object[] { false },
        };
    }

    public static User CreateValidUser(string username = "TestUser", string password = "Password123")
    {
        return new User()
        {
            Username = username,
            Password = password,
            Birthday = DateOnly.FromDateTime(DateTime.Now.AddYears(-25))
        };
    }

    public static GameLobby GetValidGameLobby(User hostUser, ISystemTime systemTime, string roomName = "Test Lobby",
        bool isPrivate = false)
    {
        return new GameLobby(hostUser, isPrivate, roomName, systemTime);
    }
}
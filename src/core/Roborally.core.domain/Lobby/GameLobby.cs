using Roborally.core.application;

namespace Roborally.core.domain.Lobby;

public class GameLobby {
    private const int MaxLobbySize = 6;
    private List<User.User> JoinedUsers { get; init; }
    public Guid GameId { get; init; }
    public bool IsPrivate { get; set; }
    public Guid HostId { get; init; }
    public string GameRoomName
    {
        get => _gameRoomName;
        init
        {
            var trimmed = value?.Trim() ?? string.Empty;
            _gameRoomName = trimmed.Length switch
            {
                < 2 => throw new CustomException("Game room name must be at least 2 characters long", 400),
                > 100 => throw new CustomException("Game room name must be at most 100 characters long", 400),
                _ => trimmed
            };
        }
    }
    
    private readonly string _gameRoomName = string.Empty;

    private GameLobby() {} //for EFC
    
    public GameLobby(User.User hostUser, bool isPrivate, string gameRoomName) {
        HostId = hostUser.Id;
        JoinedUsers = new List<User.User>(MaxLobbySize) {
            // Host already enters the lobby
            hostUser
        };
        GameRoomName = gameRoomName;
        IsPrivate = isPrivate;
        GameId = Guid.CreateVersion7();
    }

    public void JoinLobby(User.User user) {
        if (JoinedUsers.Count >= MaxLobbySize) {
            throw new CustomException("Lobby is full", 400);
        }

        if (JoinedUsers.Contains(user)) {
            return;
        }

        JoinedUsers.Add(user);
    }

    public IReadOnlyList<User.User> GetUsersInLobby() {
        return JoinedUsers.AsReadOnly();
    }
}
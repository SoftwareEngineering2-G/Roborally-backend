using Roborally.core.application;

namespace Roborally.core.domain.Lobby;

public class GameLobby {
    private const int MaxLobbySize = 6;
    private List<User.User> JoinedUsers { get; init; }
    public required Guid GameId { get; init; }
    public required bool IsPrivate { get; set; }
    public Guid HostId { get; init; }

    public GameLobby(User.User hostUser, bool isPrivate) {
        HostId = hostUser.Id;
        JoinedUsers = new List<User.User>(MaxLobbySize) {
            // Host already enters the lobby
            hostUser
        };
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
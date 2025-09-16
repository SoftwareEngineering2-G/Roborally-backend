using Roborally.core.application;

namespace Roborally.core.domain.Lobby;

public class GameLobby {
    public List<User.User> JoinedUsers { get; init; }
    public required Guid GameId { get; init; }
    public required bool IsPrivate { get; set; }
    public Guid HostId { get; init; }

    public GameLobby(User.User hostUser, bool isPrivate) {
        HostId = hostUser.Id;
        JoinedUsers = new List<User.User>(6) {
            // Host already enters the lobby
            hostUser
        };
        IsPrivate = isPrivate;
        GameId = Guid.CreateVersion7();
    }

    public void JoinLobby(User.User user) {
        if (JoinedUsers.Count >= 6) {
            throw new CustomException("Lobby is full", 400);
        }

        if (JoinedUsers.Contains(user)) {
            return;
        }

        JoinedUsers.Add(user);
    }
}
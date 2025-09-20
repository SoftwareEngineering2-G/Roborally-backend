using Roborally.core.application;
using Roborally.core.domain.Bases;

namespace Roborally.core.domain.Lobby;

public class GameLobby : Entity {
    private const int MaxLobbySize = 6;
    private readonly List<User.User> _joinedUsers;
    public IReadOnlyList<User.User> JoinedUsers => _joinedUsers.AsReadOnly();
    private readonly string _name = string.Empty;

    public Guid GameId { get; init; }
    public bool IsPrivate { get; set; }
    public int MaxPlayers => MaxLobbySize;
    
    // Foreign key property - no need for navigation property
    public string HostUsername { get; init; } = string.Empty;
    
    public DateTime? StartedAt { get; set; }
    public DateTime CreatedAt { get; set; }

    public string Name {
        get => _name;
        init {
            var trimmed = value?.Trim() ?? string.Empty;
            _name = trimmed.Length switch {
                < 2 => throw new CustomException("Game room name must be at least 2 characters long", 400),
                > 100 => throw new CustomException("Game room name must be at most 100 characters long", 400),
                _ => trimmed
            };
        }
    }

    public bool IsActive => StartedAt == null;

    private GameLobby() {
        _joinedUsers = new List<User.User>();
        HostUsername = string.Empty;
        _name = string.Empty;
    } // for EFC

    public GameLobby(User.User hostUser, bool isPrivate, string name, ISystemTime systemTime) {
        HostUsername = hostUser.Username;
        _joinedUsers = new List<User.User>(MaxLobbySize) {
            // Host already enters the lobby
            hostUser
        };
        Name = name;
        IsPrivate = isPrivate;
        GameId = Guid.CreateVersion7();
        CreatedAt = systemTime.CurrentTime;
        StartedAt = null;
    }

    public void JoinLobby(User.User user) {
        if (!IsActive)
            throw new CustomException("Cannot join lobby - game has already started", 400);

        if (_joinedUsers.Count >= MaxLobbySize)
            throw new CustomException("Lobby is full", 400);

        if (_joinedUsers.Find( u => u.Username == user.Username) != null)
            throw new CustomException("User already in lobby", 400);

        _joinedUsers.Add(user);

        // Add the domain event
        var userJoinedLobbyEvent = new UserJoinedLobbyEvent() {
            GameId = this.GameId,
            NewUserUsername = user.Username
        };
        this.AddDomainEvent(userJoinedLobbyEvent);
    }

    public void LeaveLobby(User.User user) {
        if (!_joinedUsers.Contains(user))
            return;

        _joinedUsers.Remove(user);

        // Add the domain event
        var userLeftLobbyEvent = new UserLeftLobbyEvent() {
            GameId = this.GameId,
            UserUsername = user.Username
        };
        this.AddDomainEvent(userLeftLobbyEvent);
    }
}
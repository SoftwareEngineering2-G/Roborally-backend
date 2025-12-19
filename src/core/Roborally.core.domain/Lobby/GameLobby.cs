using Roborally.core.domain.Bases;
using Roborally.core.domain.Game.Gameboard;
using Roborally.core.domain.Game.Gameboard.Space;
using Roborally.core.domain.Game.Player;

namespace Roborally.core.domain.Lobby;

public class GameLobby {
    private const int MaxLobbySize = 6;
    
    private readonly List<User.User> _joinedUsers;
/// <author name="Sachin Baral 2025-09-19 13:01:07 +0200 12" />
    public IReadOnlyList<User.User> JoinedUsers => _joinedUsers.AsReadOnly();
    private readonly List<User.User> _requiredUsers;
/// <author name="Truong Son NGO 2025-11-12 15:35:28 +0100 14" />
    public IReadOnlyList<User.User> RequiredUsers => _requiredUsers.AsReadOnly();
    
    private readonly string _name = string.Empty;
    public Guid GameId { get; init; }

    public bool IsPrivate { get; init; }

    // Foreign key property - no need for navigation property
    public string HostUsername { get; set; }

    public DateTime? StartedAt { get; set; }
    public DateTime CreatedAt { get; init; }

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

/// <author name="Sachin Baral 2025-09-19 13:01:07 +0200 41" />
    private GameLobby() {
        _joinedUsers = new List<User.User>();
        _requiredUsers = new List<User.User>();
        HostUsername = string.Empty;
        _name = string.Empty;
    } // for EFC

/// <author name="Sachin Baral 2025-09-20 20:52:08 +0200 48" />
    public GameLobby(User.User hostUser, bool isPrivate, string name, ISystemTime systemTime) {
        HostUsername = hostUser.Username;
        _joinedUsers = new List<User.User>(MaxLobbySize) {
            // Host already enters the lobby
            hostUser
        };
        _requiredUsers = new List<User.User>();
        Name = name;
        IsPrivate = isPrivate;
        GameId = Guid.CreateVersion7();
        CreatedAt = systemTime.CurrentTime;
        StartedAt = null;
    }

/// <author name="Sachin Baral 2025-09-19 13:01:07 +0200 62" />
    public void JoinLobby(User.User user) {
        if (!IsActive)
            throw new CustomException("Cannot join lobby - game has already started", 400);

        if (_joinedUsers.Count >= MaxLobbySize)
            throw new CustomException("Lobby is full", 400);

        if (_joinedUsers.Find(u => u.Username == user.Username) != null)
            throw new CustomException("User already in lobby", 400);
        
        if (_requiredUsers.Count > 0 && _requiredUsers.Find(u => u.Username == user.Username) == null)
            throw new CustomException("User not allowed to join this private lobby", 403);

        _joinedUsers.Add(user);
    }

/// <author name="Sachin Baral 2025-09-19 13:01:07 +0200 78" />
    public void LeaveLobby(User.User user) {
        if (!_joinedUsers.Contains(user))
            return;

        _joinedUsers.Remove(user);
        // If the host leaves, assign a new host if there are still users in the lobby (not applicable if lobby for continuing a paused game)
        if (user.Username == HostUsername && _joinedUsers.Count > 0 && _requiredUsers.Count == 0) {
            HostUsername = _joinedUsers[0].Username;
        }

    }

/// <author name="Sachin Baral 2025-10-01 21:53:45 +0200 90" />
    public Game.Game StartGame(string username, ISystemTime systemTime, GameBoard gameBoard) {
        if (!username.ToLower().Equals(HostUsername.ToLower())) {
            throw new CustomException("Only the host can start the game", 403);
        }

        if (this.StartedAt is not null) {
            throw new CustomException("Cannot start game - game has already started", 400);
        }

        Robot[] robots = Robot.All();


        List<Position> spawnPositions = gameBoard.GetPositionsForSpaceType(SpaceFactory.SpawnPointName);


        List<Player> players = this._joinedUsers.Select((user, index) =>
            new Player(user.Username, this.GameId, spawnPositions[index], robots[index])).ToList();

        DateTime currentTime = systemTime.CurrentTime;

        Game.Game game = new Game.Game(this.GameId, HostUsername, Name,players, gameBoard, IsPrivate, currentTime);

        this.StartedAt = currentTime;
        this._joinedUsers.Clear();
        return game;
    }
    
/// <author name="Truong Son NGO 2025-11-12 15:35:28 +0100 117" />
    public void ContinueGame(string username, ISystemTime systemTime) {
        if (!username.ToLower().Equals(HostUsername.ToLower())) 
            throw new CustomException("Only the host can continue the game", 403);
        
        if (StartedAt is not null) 
            throw new CustomException("Cannot continue game - game is already active", 400);
        
        this.StartedAt = systemTime.CurrentTime;
        this._joinedUsers.Clear();
    }
    
/// <author name="Truong Son NGO 2025-11-12 15:35:28 +0100 128" />
    public void InitLobbyToContinue(List<User.User> users) {
        _requiredUsers.Clear();
        _requiredUsers.AddRange(users);
        StartedAt = null;
    }
}
namespace Roborally.infrastructure.broadcaster;

public static class GroupName {

/// <author name="Sachin Baral 2025-10-01 21:53:45 +0200 5" />
    public static string IndividualPlayer(string username, string gameId) => $"player-{username}-game-{gameId}";
/// <author name="Sachin Baral 2025-10-01 21:53:45 +0200 6" />
    public static string Game(string gameId) => $"game-{gameId}";
/// <author name="Sachin Baral 2025-10-01 21:53:45 +0200 7" />
    public static string GameLobby(string gameId) => $"lobby-{gameId}";

}
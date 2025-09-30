namespace Roborally.infrastructure.broadcaster;

public static class GroupName {

    public static string IndividualPlayer(string username, string gameId) => $"player-{username}-game-{gameId}";
    public static string Game(string gameId) => $"game-{gameId}";
    public static string GameLobby(string gameId) => $"lobby-{gameId}";

}
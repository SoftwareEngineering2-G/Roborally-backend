using FastEndpoints;
using Roborally.core.application.CommandContracts;

namespace Roborally.webapi.RestEndpoints.Lobby;

public class GetUserPausedGames : Endpoint<GetUserPausedGamesRequest, List<GetUserPausedGamesResponse>> {
    public override void Configure() {
        Get("/games/paused");
    }

    public override async Task HandleAsync(GetUserPausedGamesRequest request, CancellationToken ct) {
        GetUserPausedGamesCommand command = new GetUserPausedGamesCommand() {
            Username = request.Username,
        };
        
        var games = await command.ExecuteAsync(ct);
        var response = games.Select(g => new GetUserPausedGamesResponse() {
            GameId = g.GameId,
            HostUsername = g.HostUsername,
            GameRoomName = g.Name,
            PlayerUsernames = g.PlayerUsernames,
        }).ToList();

        await Send.OkAsync(response, ct);
    }
}

public class GetUserPausedGamesRequest {
    public string Username { get; set; }
}

public class GetUserPausedGamesResponse {
    public Guid GameId { get; set; }
    public string GameRoomName { get; set; }
    public string HostUsername { get; set; }
    public string[] PlayerUsernames { get; set; }
}
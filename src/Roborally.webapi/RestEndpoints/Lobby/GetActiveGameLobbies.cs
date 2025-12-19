using FastEndpoints;
using Roborally.core.application.CommandContracts;

namespace Roborally.webapi.RestEndpoints.Lobby;

public class GetActiveGameLobbies : EndpointWithoutRequest<IList<GetActiveGameLobbiesResponse>> {
/// <author name="Sachin Baral 2025-09-21 21:45:34 +0200 7" />
    public override void Configure() {
        Get("/game-lobbies");
    }

/// <author name="Sachin Baral 2025-09-21 21:45:34 +0200 11" />
    public override async Task HandleAsync(CancellationToken ct) {
        GetActiveGameLobbiesCommand command = new GetActiveGameLobbiesCommand() { };
        var lobbies = await command.ExecuteAsync(ct);
        var response = lobbies.Select(lb => new GetActiveGameLobbiesResponse() {
            CurrentAmountOfPlayers = lb.CurrentAmountOfPlayers,
            GameId = lb.GameId,
            HostUsername = lb.HostUsername,
            GameRoomName = lb.Name
        }).ToList();

        await Send.OkAsync(response, ct);
    }
}

public class GetActiveGameLobbiesResponse {
    public Guid GameId { get; set; }
    public string GameRoomName { get; set; }
    public string HostUsername { get; set; }
    public int CurrentAmountOfPlayers { get; set; }
}
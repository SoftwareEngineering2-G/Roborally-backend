using FastEndpoints;
using Roborally.core.application.CommandContracts.Game;

namespace Roborally.webapi.RestEndpoints.Game.Queries;

public class GetCurrentGameState : Endpoint<GetCurrentGameStateRequest, GetCurrentGameStateResponse> {
    public override void Configure() {
        Get("/games/{gameId}/current-state");
    }

    public override async Task HandleAsync(GetCurrentGameStateRequest req, CancellationToken ct) {
        GetCurrentGameStateCommand command = new GetCurrentGameStateCommand() {
            GameId = req.GameId
        };
        
        var response = await command.ExecuteAsync(ct);
        await Send.OkAsync(new GetCurrentGameStateResponse() {
            GameId = response.GameId,
            CurrentPhase = response.CurrentPhase,
            Players =
                response.Players.Select(p => new GetCurrentGameStateResponse.Player(p.Username, p.Robot)).ToList(),
        }, ct);
    }
}

public class GetCurrentGameStateRequest {
    public Guid GameId { get; set; }
}

public class GetCurrentGameStateResponse {
    public string GameId { get; set; }
    public List<Player> Players { get; set; } = [];
    public string CurrentPhase { get; set; }

    // TODO:  We probably need information about gameboards, current positions and stuff

    public record Player(string Username, string Robot);
}
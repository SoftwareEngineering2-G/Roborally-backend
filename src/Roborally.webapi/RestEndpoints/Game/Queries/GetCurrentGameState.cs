using FastEndpoints;
using Roborally.core.application.CommandContracts.Game;
using Roborally.core.domain.Game.Gameboard;

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
            HostUsername = response.HostUsername,
            Name = response.Name,
            CurrentPhase = response.CurrentPhase,
            GameBoard = response.GameBoard,
            Players =
                response.Players.Select(p => new GetCurrentGameStateResponse.Player(p.Username, p.Robot)).ToList(),
        }, ct);
    }
}

public class GetCurrentGameStateRequest {
    public Guid GameId { get; set; }
}

public class GetCurrentGameStateResponse {
    public required string GameId { get; set; }
    public required List<Player> Players { get; set; } = [];
    public required string CurrentPhase { get; set; }

    public required string HostUsername { get; set; }

    public required string Name { get; set; }

    // TODO:  We probably need information about gameboards, current positions and stuff
    public GameBoard GameBoard { get; set; }

    public record Player(string Username, string Robot);
}
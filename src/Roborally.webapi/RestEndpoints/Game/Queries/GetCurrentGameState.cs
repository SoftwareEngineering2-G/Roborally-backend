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
            HostUsername = response.HostUsername,
            Name = response.Name,
            CurrentPhase = response.CurrentPhase,
            GameBoard = new GetCurrentGameStateResponse.GameBoardSpaces(response.GameBoard.Name,
                response.GameBoard.Space.Select(row => row.Select(space => new GetCurrentGameStateResponse.Space(space.Name())).ToArray()).ToArray()),
            Players = response.Players.Select(player => new GetCurrentGameStateResponse.Player(
                player.Username,
                player.Robot,
                player.CurrentFacingDirection,
                player.CurrentPosition is null ? null : new GetCurrentGameStateResponse.Position(player.CurrentPosition.X, player.CurrentPosition.Y)
            )).ToList()
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
    public GameBoardSpaces GameBoard { get; set; }
    
    public record GameBoardSpaces(string Name, Space[][] Spaces);

    public record Space(string Name);

    public record Player(string Username, string Robot,string CurrentFacingDirection, Position CurrentPosition);
    public record Position(int X, int Y);
}
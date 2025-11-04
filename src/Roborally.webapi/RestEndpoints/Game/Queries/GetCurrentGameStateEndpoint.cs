using FastEndpoints;
using Roborally.core.application.CommandContracts.Game;

namespace Roborally.webapi.RestEndpoints.Game.Queries;

public class GetCurrentGameStateEndpoint : Endpoint<GetCurrentGameStateRequest, GetCurrentGameStateCommandResponse> {
    public override void Configure() {
        Get("/games/{gameId}/current-state");
    }

    public override async Task HandleAsync(GetCurrentGameStateRequest req, CancellationToken ct) {
        GetCurrentGameStateCommand command = new GetCurrentGameStateCommand() {
            GameId = req.GameId,
            Username = req.Username,
        };

        GetCurrentGameStateCommandResponse response = await command.ExecuteAsync(ct);
        await Send.OkAsync(response, ct);
    }
}

public class GetCurrentGameStateRequest {
    public Guid GameId { get; set; }
    public string Username { get; set; }
}
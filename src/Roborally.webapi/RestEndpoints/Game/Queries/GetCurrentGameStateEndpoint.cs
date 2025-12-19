using FastEndpoints;
using Roborally.core.application.CommandContracts.Game;

namespace Roborally.webapi.RestEndpoints.Game.Queries;

public class GetCurrentGameStateEndpoint : Endpoint<GetCurrentGameStateRequest, GetCurrentGameStateCommandResponse> {
/// <author name="Sachin Baral 2025-11-04 21:24:56 +0100 7" />
    public override void Configure() {
        Get("/games/{gameId}/current-state");
    }

/// <author name="Sachin Baral 2025-11-04 21:24:56 +0100 11" />
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
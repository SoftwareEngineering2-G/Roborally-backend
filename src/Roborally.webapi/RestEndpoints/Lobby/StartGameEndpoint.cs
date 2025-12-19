using FastEndpoints;
using Roborally.core.application.CommandContracts;

namespace Roborally.webapi.RestEndpoints.Lobby;

public class StartGameEndpoint : Endpoint<StartGameRequest> {
/// <author name="Sachin Baral 2025-09-23 16:46:52 +0200 7" />
    public override void Configure() {
        Post("/game-lobbies/{gameId}/start");
    }


/// <author name="Sachin Baral 2025-09-23 16:46:52 +0200 12" />
    public override async Task HandleAsync(StartGameRequest req, CancellationToken ct) {
        StartGameCommand command = new StartGameCommand() {
            GameId = req.GameId,
            Username = req.Username,
            GameBoardName = req.GameBoardName
        };

        await command.ExecuteAsync(ct);

        await Send.OkAsync(cancellation:ct);
    }
}

public class StartGameRequest {
    public string Username { get; set; }
    public Guid GameId { get; set; }
    public string GameBoardName { get; set; }
}
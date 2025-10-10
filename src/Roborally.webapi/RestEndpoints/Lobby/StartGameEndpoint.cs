using FastEndpoints;
using Roborally.core.application.CommandContracts;

namespace Roborally.webapi.RestEndpoints.Lobby;

public class StartGameEndpoint : Endpoint<StartGameRequest> {
    public override void Configure() {
        Post("/game-lobbies/{gameId}/start");
    }


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
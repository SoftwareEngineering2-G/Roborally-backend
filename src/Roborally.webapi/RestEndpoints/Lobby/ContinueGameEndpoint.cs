using FastEndpoints;
using Roborally.core.application.CommandContracts;

namespace Roborally.webapi.RestEndpoints.Lobby;

public class ContinueGameEndpoint : Endpoint<ContinueGameRequest> {
    public override void Configure() {
        Post("/game-lobbies/{gameId}/continue");
    }


    public override async Task HandleAsync(ContinueGameRequest req, CancellationToken ct) {
        ContinueGameCommand command = new ContinueGameCommand() {
            GameId = req.GameId,
            Username = req.Username,
        };

        await command.ExecuteAsync(ct);

        await Send.OkAsync(cancellation:ct);
    }
}

public class ContinueGameRequest {
    public string Username { get; set; }
    public Guid GameId { get; set; }
}
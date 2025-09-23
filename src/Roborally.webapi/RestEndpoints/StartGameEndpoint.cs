using FastEndpoints;

namespace Roborally.webapi.RestEndpoints;

public class StartGameEndpoint : Endpoint<StartGameRequest>{
    public override void Configure() {
        Post("/games/{gameId}");
    }


    public override Task HandleAsync(StartGameRequest req, CancellationToken ct) {
        return base.HandleAsync(req, ct);
    }
}

public class StartGameRequest {
    public string Username { get; set; }
    public Guid GameId { get; set; }
}
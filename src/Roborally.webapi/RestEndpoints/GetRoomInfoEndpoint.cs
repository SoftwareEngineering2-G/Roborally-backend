using FastEndpoints;
using Roborally.core.application.CommandContracts;

namespace Roborally.webapi.RestEndpoints;

public class GetRoomInfoEndpoint : Endpoint<GetRoomInfoRequest, GetRoomInfoResponse> {
    public override void Configure() {
        Get("/game-lobbies/{GameId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetRoomInfoRequest req, CancellationToken ct) {
        GetRoomInfoCommand command = new GetRoomInfoCommand() {
            GameId = req.GameId,
            Username = req.Username
        };
    }
}

public class GetRoomInfoRequest {
    [FromHeader] public string Username { get; set; }
    public Guid GameId { get; set; }
}

public class GetRoomInfoResponse {
    public Guid GameId { get; set; }
    public List<string> ConnectedUsernames { get; set; }
}
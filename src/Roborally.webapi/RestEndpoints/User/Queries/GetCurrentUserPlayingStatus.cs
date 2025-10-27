using FastEndpoints;

namespace Roborally.webapi.RestEndpoints.User.Queries;

public class GetCurrentUserPlayingStatus : Endpoint<GetCurrentUserPlayingStatusRequest, GetCurrentUserPlayingStatusResponse>{
    public override void Configure() {
        Get("/users/{username}/isCurrentlyOnAGame");
    }

    public override Task HandleAsync(GetCurrentUserPlayingStatusRequest req, CancellationToken ct) {
        return base.HandleAsync(req, ct);
    }
}

public class GetCurrentUserPlayingStatusResponse {
    public bool IsCurrentlyOnAGame { get; set; }
    public bool IsCurrentlyOnAGameLobby { get; set; }
    public Guid? GameId { get; set; }
}

public class GetCurrentUserPlayingStatusRequest {
    public string Username { get; set; } = string.Empty;
}
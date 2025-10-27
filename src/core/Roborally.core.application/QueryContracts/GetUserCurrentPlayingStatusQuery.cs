using FastEndpoints;

namespace Roborally.core.application.QueryContracts;

public class GetUserCurrentPlayingStatusQuery : ICommand<GetCurrentUserPlayingStatusResponse> {
    public required string Username { get; init; }
}

public class GetCurrentUserPlayingStatusResponse {
    public bool IsCurrentlyOnAGame { get; set; }
    public bool IsCurrentlyOnAGameLobby { get; set; }
    public Guid? GameId { get; set; }
}
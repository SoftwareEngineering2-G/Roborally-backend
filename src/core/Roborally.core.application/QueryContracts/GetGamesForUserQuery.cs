using FastEndpoints;

namespace Roborally.core.application.QueryContracts;

public class GetGamesForUserQuery : ICommand<List<GetGamesForUserResponse>> {

    public required string Username { get; set; }
    public bool? IsPrivate { get; set; }
    public bool? IsFinished { get; set; }
    public DateOnly? From { get; set; }
    public DateOnly? To { get; set; }
    public string? SearchTag { get; set; }


}

public class GetGamesForUserResponse {
    public required Guid GameId { get; set; }
    public required string GameRoomName { get; set; }
    public required string HostUsername { get; set; }
    public required DateOnly StartDate { get; set; }
    public required bool IsFinished { get; set; }
    public required bool IsPrivate { get; set; }
    public required string? Winner { get; set; }
}
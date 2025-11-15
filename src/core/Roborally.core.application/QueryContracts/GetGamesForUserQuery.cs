using FastEndpoints;

namespace Roborally.core.application.QueryContracts;

public class GetGamesForUserQuery : ICommand<GetGamesForUserQueryResult> {

    public required string Username { get; set; }
    public bool? IsPrivate { get; set; }
    public bool? IsFinished { get; set; }
    public DateOnly? From { get; set; }
    public DateOnly? To { get; set; }
    public string? SearchTag { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetGamesForUserQueryResult {
    public required List<GetGamesForUserResponse> Items { get; set; }
    public required int TotalCount { get; set; }
    public required int TotalPages { get; set; }
    public required int CurrentPage { get; set; }
    public required int PageSize { get; set; }
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
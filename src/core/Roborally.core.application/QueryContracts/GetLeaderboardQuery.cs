using FastEndpoints;

namespace Roborally.core.application.QueryContracts;

public class GetLeaderboardQuery : ICommand<GetLeaderboardQueryResult>{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetLeaderboardQueryResult {
    public required List<GetLeaderboardQueryResponse> Items { get; set; }
    public required int TotalCount { get; set; }
    public required int TotalPages { get; set; }
    public required int CurrentPage { get; set; }
    public required int PageSize { get; set; }
}

public class GetLeaderboardQueryResponse {
    public required string Username { get; set; }
    public required int Rating { get; set; }
}
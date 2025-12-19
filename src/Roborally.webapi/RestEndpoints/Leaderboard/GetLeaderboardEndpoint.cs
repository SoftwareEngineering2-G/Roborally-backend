using FastEndpoints;
using Roborally.core.application.QueryContracts;

namespace Roborally.webapi.RestEndpoints.Leaderboard;

public class GetLeaderboardEndpoint : Endpoint<GetLeaderboardRequest, GetLeaderboardEndpointResponse> {
/// <author name="Sachin Baral 2025-11-15 19:46:26 +0100 7" />
    public override void Configure() {
        Get("/leaderboard");
        AllowAnonymous();
    }

/// <author name="Sachin Baral 2025-11-15 19:46:26 +0100 12" />
    public override async Task HandleAsync(GetLeaderboardRequest req, CancellationToken ct) {
        GetLeaderboardQuery query = new GetLeaderboardQuery() {
            PageNumber = req.PageNumber,
            PageSize = req.PageSize
        };

        var result = await query.ExecuteAsync(ct);

        var response = new GetLeaderboardEndpointResponse {
            Items = result.Items.Select(user => new GetLeaderboardResponse() {
                Username = user.Username,
                Rating = user.Rating
            }).ToList(),
            TotalCount = result.TotalCount,
            TotalPages = result.TotalPages,
            CurrentPage = result.CurrentPage,
            PageSize = result.PageSize
        };

        await Send.OkAsync(response, ct);
    }
}

public class GetLeaderboardRequest {
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetLeaderboardEndpointResponse {
    public required List<GetLeaderboardResponse> Items { get; set; }
    public required int TotalCount { get; set; }
    public required int TotalPages { get; set; }
    public required int CurrentPage { get; set; }
    public required int PageSize { get; set; }
}

public class GetLeaderboardResponse {
    public required string Username { get; set; }
    public required int Rating { get; set; }
}
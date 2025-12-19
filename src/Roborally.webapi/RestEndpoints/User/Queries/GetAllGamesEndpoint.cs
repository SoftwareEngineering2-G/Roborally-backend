using FastEndpoints;
using Roborally.core.application.QueryContracts;

namespace Roborally.webapi.RestEndpoints.User.Queries;

public class GetAllGamesEndpoint : Endpoint<GetAllGamesRequest, GetAllGamesEndpointResponse> {
/// <author name="Sachin Baral 2025-10-27 17:05:05 +0100 7" />
    public override void Configure() {
        Get("/users/{username}/games");
    }

/// <author name="Sachin Baral 2025-10-27 17:05:05 +0100 11" />
    public override async Task HandleAsync(GetAllGamesRequest req, CancellationToken ct) {
        GetGamesForUserQuery query = new GetGamesForUserQuery() {
            Username = req.Username,
            From = req.From,
            To = req.To,
            IsFinished = req.IsFinished,
            IsPrivate = req.IsPrivate,
            SearchTag = req.SearchTag,
            PageNumber = req.PageNumber,
            PageSize = req.PageSize
        };

        var result = await query.ExecuteAsync(ct);

        var response = new GetAllGamesEndpointResponse {
            Items = result.Items.Select(game => new GetAllGamesResponse() {
                GameId = game.GameId,
                GameRoomName = game.GameRoomName,
                HostUsername = game.HostUsername,
                StartDate = game.StartDate,
                IsFinished = game.IsFinished,
                IsPrivate = game.IsPrivate,
                Winner = game.Winner,
            }).ToList(),
            TotalCount = result.TotalCount,
            TotalPages = result.TotalPages,
            CurrentPage = result.CurrentPage,
            PageSize = result.PageSize
        };

        await Send.OkAsync(response, ct);
    }
}

public class GetAllGamesRequest {
    public string Username { get; set; } = string.Empty;
    public bool? IsPrivate { get; set; }
    public bool? IsFinished { get; set; }
    public DateOnly? From { get; set; }
    public DateOnly? To { get; set; }
    public string? SearchTag { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetAllGamesEndpointResponse {
    public required List<GetAllGamesResponse> Items { get; set; }
    public required int TotalCount { get; set; }
    public required int TotalPages { get; set; }
    public required int CurrentPage { get; set; }
    public required int PageSize { get; set; }
}

public class GetAllGamesResponse {
    public required Guid GameId { get; set; }
    public required string GameRoomName { get; set; }
    public required string HostUsername { get; set; }
    public required DateOnly StartDate { get; set; }
    public required bool IsFinished { get; set; }
    public required string? Winner { get; set; }
    public required bool IsPrivate { get; set; }
}
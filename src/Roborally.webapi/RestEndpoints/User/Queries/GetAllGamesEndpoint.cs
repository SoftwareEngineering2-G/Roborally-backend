using FastEndpoints;
using Roborally.core.application.QueryContracts;

namespace Roborally.webapi.RestEndpoints.User.Queries;

public class GetAllGamesEndpoint : Endpoint<GetAllGamesRequest, List<GetAllGamesResponse>> {
    public override void Configure() {
        Get("/users/{username}/games");
    }

    public override async Task HandleAsync(GetAllGamesRequest req, CancellationToken ct) {
        GetGamesForUserQuery query = new GetGamesForUserQuery() {
            Username = req.Username,
            From = req.From,
            To = req.To,
            IsFinished = req.IsFinished,
            IsPrivate = req.IsPrivate,
        };

        var response = await query.ExecuteAsync(ct);
        var responsePacker = response.Select(game => new GetAllGamesResponse() {
            GameId = game.GameId,
            GameRoomName = game.GameRoomName,
            HostUsername = game.HostUsername,
            StartDate = game.StartDate
        }).ToList();

        await Send.OkAsync(responsePacker, ct);
    }
}

public class GetAllGamesRequest {
    public string Username { get; set; } = string.Empty;
    public bool? IsPrivate { get; set; }
    public bool? IsFinished { get; set; }
    public DateOnly? From { get; set; }
    public DateOnly? To { get; set; }
}

public class GetAllGamesResponse {
    public required Guid GameId { get; set; }
    public required string GameRoomName { get; set; }
    public required string HostUsername { get; set; }
    public required DateOnly StartDate { get; set; }
}
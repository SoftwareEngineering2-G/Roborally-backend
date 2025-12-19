using FastEndpoints;
using Roborally.core.application.QueryContracts;

namespace Roborally.webapi.RestEndpoints.User.Queries;

public class GetMyProfileInformation : Endpoint<GetMyProfileRequest, GetMyProfileResponse> {
/// <author name="Sachin Baral 2025-11-15 19:46:26 +0100 7" />
    public override void Configure() {
        Get("/users/{username}/profile");
    }

/// <author name="Sachin Baral 2025-11-15 19:46:26 +0100 11" />
    public override async Task HandleAsync(GetMyProfileRequest req, CancellationToken ct) {
        GetMyProfileQuery query = new GetMyProfileQuery() {
            Username = req.Username
        };

        var response = await query.ExecuteAsync(ct);

        await Send.OkAsync(new GetMyProfileResponse() {
            Username = response.Username,
            Birthday = response.Birthday,
            Rating = response.Rating
        }, ct);
    }
}

public class GetMyProfileRequest {
    public required string Username { get; set; }
}
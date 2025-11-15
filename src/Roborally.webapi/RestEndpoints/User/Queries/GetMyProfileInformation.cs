using FastEndpoints;
using Roborally.core.application.QueryContracts;

namespace Roborally.webapi.RestEndpoints.User.Queries;

public class GetMyProfileInformation : Endpoint<GetMyProfileRequest, GetMyProfileResponse> {
    public override void Configure() {
        Get("/users/{username}/profile");
    }

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
using FastEndpoints;
using Roborally.core.application.CommandContracts;

namespace Roborally.webapi.RestEndpoints.User;

public class SigninUserEndpoint : Endpoint<SigninUserRequest, SigninUserResponse> {
    public override void Configure() {
        Post("/users/signin");
    }

    public override async Task HandleAsync(SigninUserRequest req, CancellationToken ct) {
        SignInCommand command = new SignInCommand() {
            Username = req.Username,
            Password = req.Password,
        };

        string username = await command.ExecuteAsync(ct);
        await Send.OkAsync(new SigninUserResponse() {
            Username = username,
        }, ct);
    }
}

public class SigninUserRequest {
    public string Username { get; set; }
    public string Password { get; set; }
}

public class SigninUserResponse {
    public string Username { get; set; }
}

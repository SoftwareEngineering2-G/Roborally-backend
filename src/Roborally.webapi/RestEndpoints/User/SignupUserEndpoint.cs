using FastEndpoints;
using Roborally.core.application.CommandContracts;

namespace Roborally.webapi.RestEndpoints.User;

public class SignupUserEndpoint : Endpoint<SignUpUserRequest, SignUpUserResponse> {
    public override void Configure() {
        Post("/users/signup");
    }

    public override async Task HandleAsync(SignUpUserRequest req, CancellationToken ct) {
        SignupCommand command = new SignupCommand() {
            Birthday = req.Birthday,
            Password = req.Password,
            Username = req.Username
        };
        string username = await command.ExecuteAsync(ct);
        await Send.OkAsync(new SignUpUserResponse {
            Username = username
        }, ct);
    }
}

public class SignUpUserRequest {
    public string Username { get; set; }                   
    public string Password { get; set; }
    public DateOnly Birthday { get; set; }
}

public class SignUpUserResponse {
    public string Username { get; set; }
}
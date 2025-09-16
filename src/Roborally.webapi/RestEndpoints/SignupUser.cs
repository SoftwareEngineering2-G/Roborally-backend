using FastEndpoints;
using Roborally.core.application.Contracts;

namespace Roborally.webapi.RestEndpoints;

public class SignupUser : Endpoint<SignUpUserRequest, SignUpUserResponse> {
    public override void Configure() {
        Post("/users/signup");
    }

    public override async Task HandleAsync(SignUpUserRequest req, CancellationToken ct) {
        SignupCommand command = new SignupCommand() {
            Birthday = req.Birthday,
            Password = req.Password,
            Username = req.Username
        };
        Guid userid = await command.ExecuteAsync(ct);
        await Send.OkAsync(new SignUpUserResponse {
            UserId = userid
        }, ct);
    }
}

public class SignUpUserRequest {
    public string Username { get; set; }                   
    public string Password { get; set; }
    public DateOnly Birthday { get; set; }
}

public class SignUpUserResponse {
    public Guid UserId { get; set; }
}
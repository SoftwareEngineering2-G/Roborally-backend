using FastEndpoints;
using Roborally.core.application.Contracts;

namespace Roborally.webapi.RestEndpoints;

public class SigninUser : Endpoint<SigninUserRequest, SigninUserResponse> {
    public override void Configure() {
        Post("/users/signin");
    }

    public override async Task HandleAsync(SigninUserRequest req, CancellationToken ct) {
        SignInCommand command = new SignInCommand() {
            Username = req.Username,
            Password = req.Password,
        };

        Guid userid = await command.ExecuteAsync(ct);
        await Send.OkAsync(new SigninUserResponse() {
            UserId = userid,
        }, ct);
    }
}

public class SigninUserRequest {
    public string Username { get; set; }
    public string Password { get; set; }
}

public class SigninUserResponse {
    public Guid UserId { get; set; }
}                                                                                                         
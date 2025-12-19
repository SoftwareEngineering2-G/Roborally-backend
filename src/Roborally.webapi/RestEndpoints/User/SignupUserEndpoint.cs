using FastEndpoints;
using Roborally.core.application.CommandContracts;

namespace Roborally.webapi.RestEndpoints.User;

public class SignupUserEndpoint : Endpoint<SignUpUserRequest, SignUpUserResponse>
{
/// <author name="Gaurav Pandey 2025-11-27 21:00:00 +0100 8" />
    public override void Configure()
    {
        Post("/users/signup");
        AllowAnonymous(); // Anyone can sign in
    }

/// <author name="Gaurav Pandey 2025-11-27 21:00:00 +0100 14" />
    public override async Task HandleAsync(SignUpUserRequest req, CancellationToken ct)
    {
        SignupCommand command = new SignupCommand
        {
            Birthday = req.Birthday,
            Password = req.Password,
            Username = req.Username
        };
        
        // Execute
        var response = await command.ExecuteAsync(ct);
        
        // Return username and token
        await Send.OkAsync(new SignUpUserResponse
        {
            Username = response.Username,
            Token = response.Token
        }, ct);
    }
}

public class SignUpUserRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
    public DateOnly Birthday { get; set; }
}

public class SignUpUserResponse
{
    public string Username { get; set; }
    public string Token { get; set; } // JWT token
}
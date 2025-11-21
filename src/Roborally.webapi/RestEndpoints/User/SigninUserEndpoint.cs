using FastEndpoints;
using Roborally.core.application.CommandContracts;

namespace Roborally.webapi.RestEndpoints.User;

public class SigninUserEndpoint : Endpoint<SigninUserRequest, SigninUserResponse>
{
    public override void Configure()
    {
        Post("/users/signin");
        AllowAnonymous(); // Signin open for all
    }

    public override async Task HandleAsync(SigninUserRequest req, CancellationToken ct)
    {
        SignInCommand command = new SignInCommand
        {
            Username = req.Username,
            Password = req.Password,
        };

        // Execute 
        var response = await command.ExecuteAsync(ct);
        
        // SUername and tokenreturn
        await Send.OkAsync(new SigninUserResponse
        {
            Username = response.Username,
            Token = response.Token
        }, ct);
    }
}

public class SigninUserRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class SigninUserResponse
{
    public string Username { get; set; }
    public string Token { get; set; } // JWT token
}

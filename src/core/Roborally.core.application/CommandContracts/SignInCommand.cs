using FastEndpoints;

namespace Roborally.core.application.CommandContracts;

public class SignInCommand : ICommand<SignInCommandResponse>
{
    public required string Username { get; init; }
    public required string Password { get; init; }
}

public class SignInCommandResponse
{
    public required string Username { get; init; }
    public required string Token { get; init; }
}
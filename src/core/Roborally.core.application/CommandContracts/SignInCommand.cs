using FastEndpoints;

namespace Roborally.core.application.CommandContracts;

public class SignInCommand : ICommand<string> {
    public required string Username { get; init; }
    public required string Password { get; init; }
}
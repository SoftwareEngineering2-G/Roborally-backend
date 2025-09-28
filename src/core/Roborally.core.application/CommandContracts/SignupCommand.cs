using FastEndpoints;

namespace Roborally.core.application.CommandContracts;

public class SignupCommand : ICommand<string> {
    public required string Username { get; init; }
    public required string Password { get; init; }
    public required DateOnly Birthday { get; init; }
}
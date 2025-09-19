using FastEndpoints;

namespace Roborally.core.application.CommandContracts;

public class SignupCommand : ICommand<string> {
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required DateOnly Birthday { get; set; }
}
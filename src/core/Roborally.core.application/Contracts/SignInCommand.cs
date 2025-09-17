using FastEndpoints;

namespace Roborally.core.application.Contracts;

public class SignInCommand : ICommand<Guid> {
    public required string Username { get; set; }
    public required string Password { get; set; }
}
using FastEndpoints;

namespace Roborally.core.application.Contracts;

public class SignupCommand : ICommand<Guid> {
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required DateOnly Birthday { get; set; }}


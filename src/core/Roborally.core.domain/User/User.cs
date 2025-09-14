namespace Roborally.core.domain.User;
public class User {
    public Guid Id { get; init; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public DateOnly Birthday { get; set; }
}
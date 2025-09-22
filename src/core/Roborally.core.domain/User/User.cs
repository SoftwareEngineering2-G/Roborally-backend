using Roborally.core.application;
using Roborally.core.domain.Bases;

namespace Roborally.core.domain.User;

public class User : Entity{
    
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Username {
        get => _username;
        init {
            _username = value.Trim().Length switch {
                < 2 => throw new CustomException("Username must be at least 2 characters long", 400),
                > 100 => throw new CustomException("Username must be at most 100 characters long", 400),
                _ => value
            };
        }
    }
    public required string Password {
        get => _password;
        init {
            _password = value.Length switch {
                < 4 => throw new CustomException("Password must be at least 4 characters long", 400),
                > 100 => throw new CustomException("Password must be at most 100 characters long", 400),
                _ => value
            };
        }
    }

    public DateOnly Birthday { get; set; }


    private readonly string _username = string.Empty;
    private readonly string _password = string.Empty;


}
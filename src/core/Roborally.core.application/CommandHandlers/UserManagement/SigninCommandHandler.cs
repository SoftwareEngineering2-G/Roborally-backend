using BCrypt.Net;
using FastEndpoints;
using Roborally.core.application.ApplicationContracts;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.CommandContracts;
using Roborally.core.domain;
using Roborally.core.domain.User;

namespace Roborally.core.application.CommandHandlers.UserManagement;

public class SigninCommandHandler : ICommandHandler<SignInCommand, SignInCommandResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public SigninCommandHandler(
        IUserRepository userRepository,
        IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<SignInCommandResponse> ExecuteAsync(SignInCommand command, CancellationToken ct)
    {
        // Find user by username
        User? user = await _userRepository.FindAsync(command.Username, ct);
        
        if (user == null)
        {
            throw new CustomException("Invalid username or password", 401);
        }

        // Verify password using BCrypt
        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(command.Password, user.Password);
        
        if (!isPasswordValid)
        {
            throw new CustomException("Invalid username or password", 401);
        }

        // Generate JWT token
        string token = _jwtService.GenerateToken(user.Username);

        return new SignInCommandResponse
        {
            Username = user.Username,
            Token = token
        };
    }
}
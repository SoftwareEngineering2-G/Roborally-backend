using BCrypt.Net;
using FastEndpoints;
using Roborally.core.application.ApplicationContracts;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.CommandContracts;
using Roborally.core.domain;
using Roborally.core.domain.Bases;
using Roborally.core.domain.User;

namespace Roborally.core.application.CommandHandlers.UserManagement;

public class SignupCommandHandler : ICommandHandler<SignupCommand, SignupCommandResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtService _jwtService;

    public SignupCommandHandler(
        IUserRepository userRepository, 
        IUnitOfWork unitOfWork,
        IJwtService jwtService)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
    }

    public async Task<SignupCommandResponse> ExecuteAsync(SignupCommand command, CancellationToken ct)
    {
        bool alreadyExists = await _userRepository.ExistsByUsernameAsync(command.Username, ct);
        if (alreadyExists)
        {
            throw new CustomException("Username already exists", 409);
        }

        // Hash the password using BCrypt
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(command.Password);

        User user = new User
        {
            Password = hashedPassword,  // Store HASHED password
            Username = command.Username,
            Birthday = command.Birthday
        };

        await _userRepository.AddAsync(user, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        // Generate JWT token
        string token = _jwtService.GenerateToken(user.Username);

        // Return username and token
        return new SignupCommandResponse
        {
            Username = user.Username,
            Token = token
        };
    }
}
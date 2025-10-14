using FastEndpoints;
using Roborally.core.application.CommandContracts;
using Roborally.core.domain;
using Roborally.core.domain.Bases;
using Roborally.core.domain.User;

namespace Roborally.core.application.CommandHandlers.UserManagement;

public class SignupCommandHandler : ICommandHandler<SignupCommand, string> {
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SignupCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork) {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<string> ExecuteAsync(SignupCommand command, CancellationToken ct) {
        bool alreadyExists = await _userRepository.ExistsByUsernameAsync(command.Username, ct);
        if (alreadyExists) {
            throw new CustomException("Username already exists", 409);
        }

        User user = new User() {
            Password = command.Password,
            Username = command.Username,
            Birthday = command.Birthday
        };

        await _userRepository.AddAsync(user, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return user.Username;
    }
}
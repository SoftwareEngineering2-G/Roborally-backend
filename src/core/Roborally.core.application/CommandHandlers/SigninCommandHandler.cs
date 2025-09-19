using FastEndpoints;
using Roborally.core.application.CommandContracts;
using Roborally.core.domain.User;

namespace Roborally.core.application.CommandHandlers;

public class SigninCommandHandler : ICommandHandler<SignInCommand, string> {
    private readonly IUserRepository _userRepository;

    public SigninCommandHandler(IUserRepository userRepository) {
        this._userRepository = userRepository;
    }

    public async Task<string> ExecuteAsync(SignInCommand command, CancellationToken ct) {
        User? user = await _userRepository.FindAsync(command.Username, ct);

        if (user is null || !user.Password.Equals(command.Password)) {
            throw new CustomException("Invalid username or password", 401);
        }

        return user.Username;
    }
}
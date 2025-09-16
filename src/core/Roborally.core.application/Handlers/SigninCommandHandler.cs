using FastEndpoints;
using Roborally.core.application.Contracts;
using Roborally.core.domain.User;

namespace Roborally.core.application.Handlers;

public class SigninCommandHandler : ICommandHandler<SignInCommand, Guid> {
    private readonly IUserRepository _userRepository;

    public SigninCommandHandler(IUserRepository userRepository) {
        this._userRepository = userRepository;
    }

    public async Task<Guid> ExecuteAsync(SignInCommand command, CancellationToken ct) {
        User? user = await _userRepository.FindByUsernameAsync(command.Username, ct);

        if (user is null || !user.Password.Equals(command.Password)) {
            throw new CustomException("Invalid username or password", 401);
        }

        return user.Id;
    }
}
using FastEndpoints;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.QueryContracts;
using Roborally.core.domain;
using Roborally.core.domain.User;

namespace Roborally.core.application.QueryHandlers;

public class GetMyProfileQueryHandler : ICommandHandler<GetMyProfileQuery, GetMyProfileResponse>{
    private readonly IUserRepository _userRepository;
    public GetMyProfileQueryHandler(IUserRepository userRepository) {
        _userRepository = userRepository;
    }


    public async Task<GetMyProfileResponse> ExecuteAsync(GetMyProfileQuery command, CancellationToken ct) {
        User? user = await _userRepository.FindAsync(command.Username, ct);
        if (user is null) {
            throw new CustomException("User not found", 404);
        }

        return new GetMyProfileResponse() {
            Username = user.Username,
            Birthday = user.Birthday,
            Rating = user.Rating
        };

    }
}


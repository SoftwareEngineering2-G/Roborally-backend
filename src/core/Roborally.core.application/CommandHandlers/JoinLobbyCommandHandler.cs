using FastEndpoints;
using Roborally.core.application.CommandContracts;
using Roborally.core.domain.Bases;
using Roborally.core.domain.Game;
using Roborally.core.domain.Lobby;
using Roborally.core.domain.User;

namespace Roborally.core.application.CommandHandlers;

public class JoinLobbyCommandHandler : ICommandHandler<JoinLobbyCommand> {

    private readonly IGameLobbyRepository _gameLobbyRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public JoinLobbyCommandHandler(IGameLobbyRepository gameLobbyRepository, IUserRepository userRepository, IUnitOfWork unitOfWork) {
        _gameLobbyRepository = gameLobbyRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(JoinLobbyCommand command, CancellationToken ct) {
        // Get the user by username
        var user = await _userRepository.FindAsync(command.Username, ct);
        if (user is null)
            throw new CustomException("User not found", 404);

        var gameLobby = await _gameLobbyRepository.FindAsync(command.GameId);
        if (gameLobby is null)
            throw new CustomException("Game lobby not found", 404);

        // Join the lobby (this will raise the UserJoinedLobbyEvent)
        gameLobby.JoinLobby(user);

        // Save changes (this will publish the domain event and broadcast via SignalR)
        await _unitOfWork.SaveChangesAsync(ct);
    }
}
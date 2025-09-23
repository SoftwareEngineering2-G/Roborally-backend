using FastEndpoints;
using Roborally.core.application.CommandContracts;
using Roborally.core.domain.Bases;
using Roborally.core.domain.Lobby;
using Roborally.core.domain.User;

namespace Roborally.core.application.CommandHandlers;

public class StartGameCommandHandler : ICommandHandler<StartGameCommand> {
    private readonly IUserRepository _userRepository;
    private readonly IGameLobbyRepository _gameLobbyRepository;
    private IUnitOfWork _unitOfWork;
    private ISystemTime _systemTime;

    public StartGameCommandHandler(IUserRepository userRepository, IGameLobbyRepository gameLobbyRepository, IUnitOfWork unitOfWork, ISystemTime systemTime) {
        _userRepository = userRepository;
        _gameLobbyRepository = gameLobbyRepository;
        _unitOfWork = unitOfWork;
        _systemTime = systemTime;
    }


    public async Task ExecuteAsync(StartGameCommand command, CancellationToken ct) {
        bool userExists = await _userRepository.ExistsByUsernameAsync(command.Username, ct);
        if (!userExists) {
            throw new CustomException("User does not exist", 404);
        }

        GameLobby? lobby = await _gameLobbyRepository.FindAsync(command.GameId);

        if (lobby is null) {
            throw new CustomException("Game lobby does not exist", 404);
        }

        lobby.StartGame(_systemTime);
        await _unitOfWork.SaveChangesAsync(ct);
    }
}
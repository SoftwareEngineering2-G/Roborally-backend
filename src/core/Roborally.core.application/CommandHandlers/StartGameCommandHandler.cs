using FastEndpoints;
using Roborally.core.application.Broadcasters;
using Roborally.core.application.CommandContracts;
using Roborally.core.domain.Bases;
using Roborally.core.domain.Game;
using Roborally.core.domain.Lobby;
using Roborally.core.domain.User;

namespace Roborally.core.application.CommandHandlers;

public class StartGameCommandHandler : ICommandHandler<StartGameCommand> {
    private readonly IUserRepository _userRepository;
    private readonly IGameLobbyRepository _gameLobbyRepository;
    private readonly IGameRepository _gameRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISystemTime _systemTime;

    private readonly IGameLobbyBroadcaster _gameLobbyBroadcaster;

    public StartGameCommandHandler(IUserRepository userRepository, IGameLobbyRepository gameLobbyRepository,
        IUnitOfWork unitOfWork, ISystemTime systemTime, IGameLobbyBroadcaster gameLobbyBroadcaster,
        IGameRepository gameRepository) {
        _userRepository = userRepository;
        _gameLobbyRepository = gameLobbyRepository;
        _unitOfWork = unitOfWork;
        _systemTime = systemTime;
        _gameLobbyBroadcaster = gameLobbyBroadcaster;
        _gameRepository = gameRepository;
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

        Game game = lobby.StartGame(command.Username, _systemTime);
        await _gameRepository.AddAsync(game, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        await _gameLobbyBroadcaster.BroadcastGameStartedAsync(command.GameId, ct);
    }
}
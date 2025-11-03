using FastEndpoints;
using Roborally.core.application.ApplicationContracts.Broadcasters;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.CommandContracts;
using Roborally.core.domain;
using Roborally.core.domain.Bases;
using Roborally.core.domain.Game;
using Roborally.core.domain.Game.Gameboard;
using Roborally.core.domain.Lobby;
using Roborally.core.domain.User;

namespace Roborally.core.application.CommandHandlers.GameLobby;

public class StartGameCommandHandler : ICommandHandler<StartGameCommand> {
    private readonly IUserRepository _userRepository;
    private readonly IGameLobbyRepository _gameLobbyRepository;
    private readonly IGameRepository _gameRepository;
    private readonly IGameBoardRepository _gameBoardRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISystemTime _systemTime;

    private readonly IGameLobbyBroadcaster _gameLobbyBroadcaster;

    public StartGameCommandHandler(IUserRepository userRepository, IGameLobbyRepository gameLobbyRepository,
        IUnitOfWork unitOfWork, ISystemTime systemTime, IGameLobbyBroadcaster gameLobbyBroadcaster,
        IGameRepository gameRepository, IGameBoardRepository gameBoardRepository) {
        _userRepository = userRepository;
        _gameLobbyRepository = gameLobbyRepository;
        _unitOfWork = unitOfWork;
        _systemTime = systemTime;
        _gameLobbyBroadcaster = gameLobbyBroadcaster;
        _gameRepository = gameRepository;
        _gameBoardRepository = gameBoardRepository;
    }


    public async Task ExecuteAsync(StartGameCommand command, CancellationToken ct) {
        bool userExists = await _userRepository.ExistsByUsernameAsync(command.Username, ct);
        if (!userExists) {
            throw new CustomException("User does not exist", 404);
        }

        domain.Lobby.GameLobby? lobby = await _gameLobbyRepository.FindAsync(command.GameId);

        if (lobby is null) {
            throw new CustomException("Game lobby does not exist", 404);
        }

        // Get existing GameBoard from database
        GameBoard? gameBoard = await _gameBoardRepository.FindAsync(command.GameBoardName, ct);

        if (gameBoard == null) {
            throw new CustomException($"Game board '{command.GameBoardName}' does not exist", 404);
        }
        
        domain.Game.Game game = lobby.StartGame(command.Username, _systemTime, gameBoard);
        await _gameRepository.AddAsync(game, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        await _gameLobbyBroadcaster.BroadcastGameStartedAsync(command.GameId, ct);
    }
}
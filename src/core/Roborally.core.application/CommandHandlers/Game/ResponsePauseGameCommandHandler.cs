using FastEndpoints;
using Roborally.core.application.ApplicationContracts.Broadcasters;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.CommandContracts.Game;
using Roborally.core.domain;
using Roborally.core.domain.Bases;
using Roborally.core.domain.Game;

namespace Roborally.core.application.CommandHandlers.Game;

public class ResponsePauseGameCommandHandler : ICommandHandler<ResponsePauseGameCommand> {
    private readonly IGameRepository _gameRepository;
    private readonly IGameBroadcaster _gameBroadcaster;
    private readonly IUnitOfWork _unitOfWork;

    private readonly ISystemTime _systemTime;


    public ResponsePauseGameCommandHandler(IGameRepository gameRepository, IGameBroadcaster gameBroadcaster,
        IUnitOfWork unitOfWork, ISystemTime systemTime) {
        _gameRepository = gameRepository;
        _gameBroadcaster = gameBroadcaster;
        _unitOfWork = unitOfWork;
        _systemTime = systemTime;
    }

    public async Task ExecuteAsync(ResponsePauseGameCommand command, CancellationToken ct) {
        var game = await _gameRepository.FindAsync(command.GameId, ct);
        if (game is null) {
            throw new CustomException("Game not found", 404);
        }
        
        game.ResponsePauseGame(command.ResponderUsername, command.Approved, _systemTime);
        await _gameBroadcaster.BroadcastPauseGameRequestedAsync(game.GameId, command.ResponderUsername, ct);
        
        var gamePauseState = game.GetGamePauseState();
        if (gamePauseState != null)
        {
            await _gameBroadcaster.BroadcastPauseGameResultAsync(game.GameId, gamePauseState, ct);
        }
        await _unitOfWork.SaveChangesAsync(ct);
        
    }
}
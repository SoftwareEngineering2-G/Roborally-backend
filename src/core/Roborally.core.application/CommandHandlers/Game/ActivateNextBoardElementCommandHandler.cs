using FastEndpoints;
using Roborally.core.application.ApplicationContracts.Broadcasters;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.CommandContracts.Game;
using Roborally.core.domain;
using Roborally.core.domain.Bases;
using Roborally.core.domain.Game;

namespace Roborally.core.application.CommandHandlers.Game;

public class ActivateNextBoardElementCommandHandler : ICommandHandler<ActivateNextBoardElementCommand> {
    private readonly IGameRepository _gameRepository;
    private readonly IGameBroadcaster _gameBroadcaster;
    private readonly IUnitOfWork _unitOfWork;

    private readonly ISystemTime _systemTime;


    public ActivateNextBoardElementCommandHandler(IGameRepository gameRepository, IGameBroadcaster gameBroadcaster,
        IUnitOfWork unitOfWork, ISystemTime systemTime) {
        _gameRepository = gameRepository;
        _gameBroadcaster = gameBroadcaster;
        _unitOfWork = unitOfWork;
        _systemTime = systemTime;
    }

    public async Task ExecuteAsync(ActivateNextBoardElementCommand command, CancellationToken ct) {
        var game = await _gameRepository.FindAsync(command.GameId, ct);
        if (game is null) {
            throw new CustomException("Game not found", 404);
        }
        game.ActivateNextBoardElement(_systemTime);
        await _unitOfWork.SaveChangesAsync(ct);
    }
}
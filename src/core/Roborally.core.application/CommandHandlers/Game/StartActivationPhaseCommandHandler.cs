using FastEndpoints;
using Roborally.core.application.Broadcasters;
using Roborally.core.application.CommandContracts.Game;
using Roborally.core.domain.Bases;
using Roborally.core.domain.Game;

namespace Roborally.core.application.CommandHandlers.Game;

public class StartActivationPhaseCommandHandler : ICommandHandler<StartActivationPhaseCommand>
{
    private readonly IGameRepository _gameRepository;
    private readonly IGameBroadcaster _gameBroadcaster;
    private readonly IUnitOfWork _unitOfWork;
    
    public StartActivationPhaseCommandHandler(IGameRepository gameRepository, IGameBroadcaster gameBroadcaster, IUnitOfWork unitOfWork)
    {
        _gameRepository = gameRepository;
        _gameBroadcaster = gameBroadcaster;
        _unitOfWork = unitOfWork;
    }
    
    public async Task ExecuteAsync(StartActivationPhaseCommand command, CancellationToken ct)
    {
        domain.Game.Game? game =  await _gameRepository.FindAsync(command.GameId, ct);
        if (game is null)
        {
            throw new CustomException("Game not found", 405);
        }

        game.CurrentPhase = GamePhase.ActivationPhase;
        
        await _unitOfWork.SaveChangesAsync(ct);
        
        await _gameBroadcaster.BroadcastActivationPhaseStartedAsync(command.GameId, ct);
    }
}
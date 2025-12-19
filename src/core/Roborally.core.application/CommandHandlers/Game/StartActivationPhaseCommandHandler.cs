using FastEndpoints;
using Roborally.core.application.ApplicationContracts.Broadcasters;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.CommandContracts.Game;
using Roborally.core.domain;
using Roborally.core.domain.Bases;
using Roborally.core.domain.Game;

namespace Roborally.core.application.CommandHandlers.Game;

public class StartActivationPhaseCommandHandler : ICommandHandler<StartActivationPhaseCommand>
{
    private readonly IGameRepository _gameRepository;
    private readonly IGameBroadcaster _gameBroadcaster;
    private readonly IUnitOfWork _unitOfWork;
    
/// <author name="Suhani Pandey 2025-10-10 13:01:53 +0200 17" />
    public StartActivationPhaseCommandHandler(IGameRepository gameRepository, IGameBroadcaster gameBroadcaster, IUnitOfWork unitOfWork)
    {
        _gameRepository = gameRepository;
        _gameBroadcaster = gameBroadcaster;
        _unitOfWork = unitOfWork;
    }
    
/// <author name="Suhani Pandey 2025-10-10 13:01:53 +0200 24" />
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
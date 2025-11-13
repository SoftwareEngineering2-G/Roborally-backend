using FastEndpoints;
using Roborally.core.application.ApplicationContracts.Broadcasters;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.CommandContracts;
using Roborally.core.domain;
using Roborally.core.domain.Bases;
using Roborally.core.domain.Game;

namespace Roborally.core.application.CommandHandlers.GameLobby;

public class EndGameCommandHandler:ICommandHandler<EndGameCommand>
{
    private readonly IGameRepository _gameRepository;
    private readonly IGameBroadcaster _gameBroadcaster;
    private readonly IUnitOfWork _unitOfWork; 

    public EndGameCommandHandler(IGameRepository gameRepository, IUnitOfWork unitOfWork, IGameBroadcaster gameBroadcaster)
    {
        _gameRepository = gameRepository;
        _unitOfWork = unitOfWork;
        _gameBroadcaster = gameBroadcaster;
    }
    
    public async Task ExecuteAsync(EndGameCommand command, CancellationToken ct)
    {
        domain.Game.Game? game = await _gameRepository.FindAsync(command.GameId, ct);
        if (game == null)
        {
            throw new CustomException("Game does not exist", 404);
        }
        
        game.CurrentPhase = GamePhase.GameOver;
        
        await _gameRepository.AddAsync(game, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        await _gameBroadcaster.BroadcastGameEndedAsync(command.GameId, ct);
    }
}
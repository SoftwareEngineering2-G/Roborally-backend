using FastEndpoints;
using Roborally.core.application.ApplicationContracts.Broadcasters;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.CommandContracts.Game;
using Roborally.core.domain;
using Roborally.core.domain.Bases;
using Roborally.core.domain.Game;

namespace Roborally.core.application.CommandHandlers.Game;

public class RevealNextRegisterCommandHandler : ICommandHandler<RevealNextRegisterCommand>
{
    private readonly IGameRepository _gameRepository;
    private readonly IGameBroadcaster _gameBroadcaster;
    private readonly IUnitOfWork _unitOfWork;

    public RevealNextRegisterCommandHandler(
        IGameRepository gameRepository, 
        IGameBroadcaster gameBroadcaster,
        IUnitOfWork unitOfWork)
    {
        _gameRepository = gameRepository;
        _gameBroadcaster = gameBroadcaster;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(RevealNextRegisterCommand command, CancellationToken ct)
    {
        domain.Game.Game? game = await _gameRepository.FindAsync(command.GameId, ct);
        
        if (game is null)
        {
            throw new CustomException("Game not found", 404);
        }

        // Verify that the user is the host
        if (!string.Equals(game.HostUsername, command.Username, StringComparison.Ordinal))
        {
            throw new CustomException("Only the host can reveal registers", 403);
        }

        // Reveal the next register and get all players' cards for that register
        var revealedCards = game.RevealNextRegister();

        await _unitOfWork.SaveChangesAsync(ct);

        // Broadcast to all players with the revealed cards
        await _gameBroadcaster.BroadcastRegisterRevealedAsync(
            command.GameId, 
            game.CurrentRevealedRegister, 
            revealedCards, 
            ct);

        var nextPlayer = game.GetNextExecutingPlayer();
        await _gameBroadcaster.BroadcastNextPlayerInTurn(command.GameId, nextPlayer?.Username, ct);
    }
}

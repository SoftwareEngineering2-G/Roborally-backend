using FastEndpoints;
using Roborally.core.application.ApplicationContracts.Broadcasters;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.CommandContracts.PlayerEvents;
using Roborally.core.domain;
using Roborally.core.domain.Bases;
using Roborally.core.domain.Game;
using Roborally.core.domain.Game.Deck;

namespace Roborally.core.application.CommandHandlers.PlayerEvents;

public class RegistersProgrammedCommandHandler : ICommandHandler<RegistersProgrammedCommand> {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGameRepository _gameRepository;
    private readonly ISystemTime _systemTime;
    private readonly IGameBroadcaster _gameBroadcaster;

    
    public RegistersProgrammedCommandHandler(IUnitOfWork unitOfWork,
        ISystemTime systemTime, IGameBroadcaster gameBroadcaster, IGameRepository gameRepository) {
        _unitOfWork = unitOfWork;
        _systemTime = systemTime;
        _gameBroadcaster = gameBroadcaster;
        _gameRepository = gameRepository;
    }

    public async Task ExecuteAsync(RegistersProgrammedCommand command, CancellationToken ct) {
        domain.Game.Game? game = await _gameRepository.FindAsync(command.GameId, ct);
        if (game is null)
        {
            throw new CustomException("Game does not exist", 404);
        }
        
        
        List<ProgrammingCard> lockedInCards =
            command.LockedInCardsInOrder.Select(ProgrammingCard.FromString).ToList();
        
        game.LockInRegisters(command.Username, lockedInCards, _systemTime);

        await _unitOfWork.SaveChangesAsync(ct);

        // Broadcast that the player has locked in their registers
        await _gameBroadcaster.BroadcastPlayerLockedInRegisterAsync(command.Username,
            command.GameId, ct);
    }
}
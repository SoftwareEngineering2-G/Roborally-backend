using FastEndpoints;
using Roborally.core.application.Broadcasters;
using Roborally.core.application.CommandContracts.PlayerEvents;
using Roborally.core.domain.Bases;
using Roborally.core.domain.Deck;
using Roborally.core.domain.Game.Player;

namespace Roborally.core.application.CommandHandlers.PlayerEvents;

public class RegistersProgrammedCommandHandler : ICommandHandler<RegistersProgrammedCommand> {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPlayerRepository _playerRepository;
    private readonly ISystemTime _systemTime;
    private readonly IGameBroadcaster _gameBroadcaster;


    public RegistersProgrammedCommandHandler(IUnitOfWork unitOfWork, IPlayerRepository playerRepository,
        ISystemTime systemTime, IGameBroadcaster gameBroadcaster) {
        _unitOfWork = unitOfWork;
        _playerRepository = playerRepository;
        _systemTime = systemTime;
        _gameBroadcaster = gameBroadcaster;
    }

    public async Task ExecuteAsync(RegistersProgrammedCommand programmedCommand, CancellationToken ct) {
        Player? player = await _playerRepository.FindAsync(programmedCommand.Username, programmedCommand.GameId, ct);
        if (player is null) {
            throw new CustomException("Player not found", 400);
        }


        List<ProgrammingCard> lockedInCards =
            programmedCommand.LockedInCardsInOrder.Select(ProgrammingCard.FromString).ToList();
        player.LockInRegisters(lockedInCards, _systemTime);

        // Broadcast that the player has locked in their registers
        await _gameBroadcaster.BroadcastPlayerLockedInRegisterAsync(programmedCommand.Username,
            programmedCommand.GameId, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }
}
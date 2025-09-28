using FastEndpoints;
using Roborally.core.application.CommandContracts.PlayerEvents;
using Roborally.core.domain.Bases;
using Roborally.core.domain.Deck;
using Roborally.core.domain.Game.Player;

namespace Roborally.core.application.CommandHandlers.PlayerEvents;

public class RegistersProgrammedCommandHandler : ICommandHandler<RegistersProgrammedCommand> {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPlayerRepository _playerRepository;
    private readonly ISystemTime _systemTime;


    public RegistersProgrammedCommandHandler(IUnitOfWork unitOfWork, IPlayerRepository playerRepository,
        ISystemTime systemTime) {
        _unitOfWork = unitOfWork;
        _playerRepository = playerRepository;
        _systemTime = systemTime;
    }

    public async Task ExecuteAsync(RegistersProgrammedCommand programmedCommand, CancellationToken ct) {
        Player? player = await _playerRepository.FindAsync(programmedCommand.Username, programmedCommand.GameId, ct);
        if (player is null) {
            throw new CustomException("Player not found", 400);
        }


        List<ProgrammingCard> lockedInCards =
            programmedCommand.LockedInCardsInOrder.Select(ProgrammingCard.FromString).ToList();
        player.LockInRegisters(lockedInCards, _systemTime);
        await _unitOfWork.SaveChangesAsync(ct);
    }
}
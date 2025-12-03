using FastEndpoints;
using Roborally.core.application.ApplicationContracts.Broadcasters;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.CommandContracts.Game;
using Roborally.core.domain;
using Roborally.core.domain.Bases;
using Roborally.core.domain.Game.GameEvents;

namespace Roborally.core.application.CommandHandlers.Game;

public class StartNextRoundCommandHandler : ICommandHandler<StartNextRoundCommand> {
    private readonly IGameRepository _gameRepository;
    private readonly IGameBroadcaster _gameBroadcaster;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISystemTime _systemTime;

    public StartNextRoundCommandHandler(IGameRepository gameRepository, IGameBroadcaster gameBroadcaster,
        IUnitOfWork unitOfWork, ISystemTime systemTime) {
        _gameRepository = gameRepository;
        _gameBroadcaster = gameBroadcaster;
        _unitOfWork = unitOfWork;
        _systemTime = systemTime;
    }

    public async Task ExecuteAsync(StartNextRoundCommand command, CancellationToken ct) {
        var game = await _gameRepository.FindAsync(command.GameId, ct);
        if (game is null) {
            throw new CustomException("Game not found", 404);
        }

        // Start the next round
        game.StartNextRound(_systemTime);

        // Save changes
        await _unitOfWork.SaveChangesAsync(ct);

        // Get the round completed event that was just added
        var roundCompletedEvent = game.GameEvents.OfType<RoundCompletedEvent>()
            .OrderByDescending(e => e.HappenedAt)
            .FirstOrDefault();

        if (roundCompletedEvent != null) {
            // Broadcast that the round has been completed and a new round started
            await _gameBroadcaster.BroadcastRoundCompletedAsync(
                command.GameId,
                roundCompletedEvent.CompletedRound,
                roundCompletedEvent.NewRound,
                ct
            );
        }
    }
}

using Roborally.core.application.ApplicationContracts.Broadcasters;
using Roborally.core.application.ApplicationContracts.GameTimer;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.domain.Bases;

namespace Roborally.core.application.Services;

public class ProgrammingTimeoutHandler : IProgrammingTimeoutHandler
{
    private readonly IGameRepository _gameRepository;
    private readonly IGameBroadcaster _gameBroadcaster;
    private readonly IIndividualPlayerBroadcaster _individualPlayerBroadcaster;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISystemTime _systemTime;

    public ProgrammingTimeoutHandler(IGameRepository gameRepository, IGameBroadcaster gameBroadcaster,
        IIndividualPlayerBroadcaster individualPlayerBroadcaster, IUnitOfWork unitOfWork, ISystemTime systemTime)
    {
        _gameRepository = gameRepository;
        _gameBroadcaster = gameBroadcaster;
        _individualPlayerBroadcaster = individualPlayerBroadcaster;
        _unitOfWork = unitOfWork;
        _systemTime = systemTime;
    }

/// <author name="Vincenzo Altaserse 2025-12-18 17:40:31 +0100 24" />
    public async Task HandleTimeoutAsync(Guid gameId, CancellationToken ct)
    {
        var game = await _gameRepository.FindAsync(gameId, ct);
        if (game is null) return;

        var assignedCards = game.AutoCompleteEmptyRegisters(_systemTime);
        await _unitOfWork.SaveChangesAsync(ct);

        // Broadcast the assigned cards to the players who have not locked-in their register yet     
        var tasks = assignedCards
            .Select(item =>
            {
                return _individualPlayerBroadcaster.BroadcastProgrammingTimeoutAsync(
                    item.Key,
                    gameId,
                    item.Value.Select(card => card.DisplayName).ToList(), 
                    ct
                );
            })
            .ToList();
        await Task.WhenAll(tasks);

        // Broadcast the locked-in event to the players
        tasks = assignedCards
            .Select(item =>
            {
                return _gameBroadcaster.BroadcastPlayerLockedInRegisterAsync(
                    item.Key, 
                    null, 
                    gameId, 
                    ct
                );
            })
            .ToList();
        await Task.WhenAll(tasks);
    }
}
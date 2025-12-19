using Roborally.core.application.ApplicationContracts.Broadcasters;
using Roborally.core.application.ApplicationContracts.GameTimer;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.domain.Bases;

namespace Roborally.core.application.Services;

public class ProgrammingTimeoutHandler : IProgrammingTimeoutHandler
{
    private readonly IGameRepository _gameRepository;
    private readonly IGameBroadcaster _gameBroadcaster;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISystemTime _systemTime;

    public ProgrammingTimeoutHandler(IGameRepository gameRepository, IGameBroadcaster gameBroadcaster,
        IUnitOfWork unitOfWork, ISystemTime systemTime)
    {
        _gameRepository = gameRepository;
        _gameBroadcaster = gameBroadcaster;
        _unitOfWork = unitOfWork;
        _systemTime = systemTime;
    }

    public async Task HandleTimeoutAsync(Guid gameId, CancellationToken ct)
    {
        var game = await _gameRepository.FindAsync(gameId, ct);
        if (game is null) return;

        var assignedCards = game.AutoCompleteEmptyRegisters(_systemTime);
        await _unitOfWork.SaveChangesAsync(ct);
        await _gameBroadcaster.BroadcastProgrammingTimeoutAsync(gameId, assignedCards, ct);

        var tasks = assignedCards
            .Select(item => _gameBroadcaster.BroadcastPlayerLockedInRegisterAsync(item.Key, null, gameId, ct))
            .ToList();
        await Task.WhenAll(tasks);
    }
}
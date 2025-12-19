using FastEndpoints;
using Roborally.core.application.ApplicationContracts.Broadcasters;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.CommandContracts.Game;
using Roborally.core.domain;
using Roborally.core.domain.Bases;
using Roborally.core.domain.Game;

namespace Roborally.core.application.CommandHandlers.Game;

public class ActivateNextBoardElementCommandHandler : ICommandHandler<ActivateNextBoardElementCommand> {
    private readonly IGameRepository _gameRepository;
    private readonly IGameBroadcaster _gameBroadcaster;
    private readonly IUnitOfWork _unitOfWork;

    private readonly ISystemTime _systemTime;


    public ActivateNextBoardElementCommandHandler(IGameRepository gameRepository, IGameBroadcaster gameBroadcaster,
        IUnitOfWork unitOfWork, ISystemTime systemTime) {
        _gameRepository = gameRepository;
        _gameBroadcaster = gameBroadcaster;
        _unitOfWork = unitOfWork;
        _systemTime = systemTime;
    }

/// <author name="nilanjanadevkota 2025-10-14 19:37:00 +0200 27" />
    public async Task ExecuteAsync(ActivateNextBoardElementCommand command, CancellationToken ct) {
        var game = await _gameRepository.FindAsync(command.GameId, ct);
        if (game is null) {
            throw new CustomException("Game not found", 404);
        }

        // Store player positions before activation
        var playerPositionsBefore = game.Players
            .ToDictionary(p => p.Username, p => new {
                X = p.CurrentPosition.X,
                Y = p.CurrentPosition.Y,
                Direction = p.CurrentFacingDirection.DisplayName
            });

        // Activate board element
        await game.ActivateNextBoardElement(_systemTime);

        // Save changes first
        await _unitOfWork.SaveChangesAsync(ct);

        // Broadcast position updates for ALL players after board element activation
        var playersWithChanges = game.Players
            .Where(player => {
                if (!playerPositionsBefore.TryGetValue(player.Username, out var beforePos)) return false;
                // Check if position or direction changed
                return beforePos.X != player.CurrentPosition.X ||
                       beforePos.Y != player.CurrentPosition.Y ||
                       beforePos.Direction != player.CurrentFacingDirection.DisplayName;
            });

        var broadcastTasks = playersWithChanges
            .Select(player => _gameBroadcaster.BroadcastRobotMovedAsync(
                command.GameId,
                player.Username,
                player.CurrentPosition.X,
                player.CurrentPosition.Y,
                player.CurrentFacingDirection.DisplayName,
                "Board Element", // Indicate this was from board element activation
                ct
            ));

        // Await all broadcasts to complete
        await Task.WhenAll(broadcastTasks);
    }
}
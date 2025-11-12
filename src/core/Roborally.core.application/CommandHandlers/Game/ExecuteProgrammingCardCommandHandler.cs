using FastEndpoints;
using Roborally.core.application.ApplicationContracts.Broadcasters;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.CommandContracts.Game;
using Roborally.core.domain;
using Roborally.core.domain.Bases;
using Roborally.core.domain.Game.Deck;
using Roborally.core.domain.Game.Player;
using Roborally.core.domain.Game.GameEvents;

namespace Roborally.core.application.CommandHandlers.Game;

public class ExecuteProgrammingCardCommandHandler : ICommandHandler<ExecuteProgrammingCardCommand, ExecuteProgrammingCardCommandResponse>
{
    private readonly IGameRepository _gameRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGameBroadcaster _gameBroadcaster;
    private readonly ISystemTime _systemTime;

    public ExecuteProgrammingCardCommandHandler(IGameRepository gameRepository, IUnitOfWork unitOfWork, IGameBroadcaster gameBroadcaster, ISystemTime systemTime)
    {
        _gameRepository = gameRepository;
        _unitOfWork = unitOfWork;
        _gameBroadcaster = gameBroadcaster;
        _systemTime = systemTime;
    }

    public async Task<ExecuteProgrammingCardCommandResponse> ExecuteAsync(ExecuteProgrammingCardCommand command, CancellationToken ct)
    {
        var game = await _gameRepository.FindAsync(command.GameId, ct);
        if (game is null)
        {
            throw new CustomException("Game not found", 404);
        }

        ProgrammingCard card;
        try
        {
            card = ProgrammingCard.FromString(command.CardName);
        }
        catch (ArgumentException)
        {
            throw new CustomException($"Invalid card name: {command.CardName}", 400);
        }
        
        // Track how many checkpoint events existed before
        var checkpointEventsBefore = game.GameEvents.OfType<CheckpointReachedEvent>().Count();
        
        Player affectedPlayer = game.ExecuteProgrammingCard(command.Username, card, _systemTime);
        
        await _unitOfWork.SaveChangesAsync(ct);
        
        // Broadcast robot movement
        await _gameBroadcaster.BroadcastRobotMovedAsync(
            command.GameId, 
            command.Username, 
            affectedPlayer.CurrentPosition.X,
            affectedPlayer.CurrentPosition.Y,
            affectedPlayer.CurrentFacingDirection.DisplayName,
            card.DisplayName,
            ct);

        // Broadcast any new checkpoint events that were added
        var newCheckpointEvents = game.GameEvents.OfType<CheckpointReachedEvent>()
            .Skip(checkpointEventsBefore);
        
        foreach (var checkpointEvent in newCheckpointEvents)
        {
            await _gameBroadcaster.BroadcastCheckpointReachedAsync(
                command.GameId,
                checkpointEvent.Username,
                checkpointEvent.CheckpointNumber,
                ct
            );
        }

        Player? nextPlayer = game.GetNextExecutingPlayer();
        await _gameBroadcaster.BroadcastNextPlayerInTurn(command.GameId, nextPlayer?.Username, ct);

        return new ExecuteProgrammingCardCommandResponse
        {
            Message = $"Successfully executed {card.DisplayName}",
            PlayerState = new PlayerState
            {
                PositionX = affectedPlayer.CurrentPosition.X,
                PositionY = affectedPlayer.CurrentPosition.Y,
                Direction = affectedPlayer.CurrentFacingDirection.DisplayName
            }
        };
    }
}

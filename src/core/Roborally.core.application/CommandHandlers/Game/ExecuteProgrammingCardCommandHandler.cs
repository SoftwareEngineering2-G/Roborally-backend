using FastEndpoints;
using Roborally.core.application.ApplicationContracts.Broadcasters;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.CommandContracts.Game;
using Roborally.core.domain;
using Roborally.core.domain.Bases;
using Roborally.core.domain.Game;
using Roborally.core.domain.Game.CardActions;
using Roborally.core.domain.Game.Deck;
using Roborally.core.domain.Game.GameEvents;
using Roborally.core.domain.Game.Player;

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
        // Find the game
        var game = await _gameRepository.FindAsync(command.GameId, ct);
        if (game is null)
        {
            throw new CustomException("Game not found", 404);
        }

        // Parse the card
        ProgrammingCard card;
        try
        {
            card = ProgrammingCard.FromString(command.CardName);
        }
        catch (ArgumentException)
        {
            throw new CustomException($"Invalid card name: {command.CardName}", 400);
        }
        
        // Get count of checkpoint events before execution
        int checkpointEventsCountBefore = game.GameEvents.OfType<CheckpointReachedEvent>().Count();
        
        // Store positions BEFORE card execution to detect which robots actually moved
        var positionsBefore = game.Players.ToDictionary(
            p => p.Username,
            p => new { X = p.CurrentPosition.X, Y = p.CurrentPosition.Y, Direction = p.CurrentFacingDirection.DisplayName }
        );
        
        Player affectedPlayer = game.ExecuteProgrammingCard(command.Username, card, _systemTime);
        
        // Check if a new checkpoint event was added during this card execution
        var newCheckpointEvents = game.GameEvents
            .OfType<CheckpointReachedEvent>()
            .Skip(checkpointEventsCountBefore)
            .Where(e => e.Username == command.Username)
            .ToList();
        
        await _unitOfWork.SaveChangesAsync(ct);
        
        // Broadcast ONLY robots that actually moved (position or direction changed)
        foreach (var player in game.Players)
        {
            var before = positionsBefore[player.Username];
            bool hasMoved = before.X != player.CurrentPosition.X || 
                           before.Y != player.CurrentPosition.Y || 
                           before.Direction != player.CurrentFacingDirection.DisplayName;
            
            if (hasMoved)
            {
                // Determine if this is the card executor or a pushed robot
                string cardNameToSend = player.Username == command.Username ? card.DisplayName : "Pushed";
                
                await _gameBroadcaster.BroadcastRobotMovedAsync(
                    command.GameId, 
                    player.Username, 
                    player.CurrentPosition.X,
                    player.CurrentPosition.Y,
                    player.CurrentFacingDirection.DisplayName,
                    cardNameToSend,
                    ct);
            }
        }

        // Broadcast checkpoint reached if a new checkpoint was reached during this execution
        foreach (var checkpointEvent in newCheckpointEvents)
        {
            await _gameBroadcaster.BroadcastCheckpointReachedAsync(
                command.GameId,
                command.Username,
                checkpointEvent.CheckpointNumber,
                ct);
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

using FastEndpoints;
using Roborally.core.application.ApplicationContracts.Broadcasters;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.CommandContracts.Game;
using Roborally.core.domain;
using Roborally.core.domain.Bases;
using Roborally.core.domain.Game;
using Roborally.core.domain.Game.CardActions;
using Roborally.core.domain.Game.Deck;
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
        
        Player affectedPlayer = game.ExecuteProgrammingCard(command.Username, card, _systemTime);
        
        await _unitOfWork.SaveChangesAsync(ct);
        
        // Broadcast the robot movement to all players in the game
        await _gameBroadcaster.BroadcastRobotMovedAsync(
            command.GameId, 
            command.Username, 
            affectedPlayer.CurrentPosition.X,
            affectedPlayer.CurrentPosition.Y,
            affectedPlayer.CurrentFacingDirection.DisplayName,
            card.DisplayName,
            ct);

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

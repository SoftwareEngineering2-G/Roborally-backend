using FastEndpoints;
using Roborally.core.application.ApplicationContracts.Broadcasters;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.CommandContracts.Game;
using Roborally.core.domain;
using Roborally.core.domain.Bases;
using Roborally.core.domain.Game;
using Roborally.core.domain.Game.CardActions;
using Roborally.core.domain.Game.Deck;

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

        // Find the player
        var player = game.Players.FirstOrDefault(p => p.Username == command.Username);
        if (player is null)
        {
            throw new CustomException("Player not found in this game", 404);
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

        // Create and execute the action
        var action = ActionFactory.CreateAction(card);
        
        action.Execute(player, game, _systemTime);

        // Save changes
        await _unitOfWork.SaveChangesAsync(ct);
        
        // Broadcast the robot movement to all players in the game
        await _gameBroadcaster.BroadcastRobotMovedAsync(
            command.GameId, 
            command.Username, 
            player.CurrentPosition.X, 
            player.CurrentPosition.Y, 
            player.CurrentFacingDirection.DisplayName,
            card.DisplayName,
            ct);

        return new ExecuteProgrammingCardCommandResponse
        {
            Message = $"Successfully executed {card.DisplayName}",
            PlayerState = new PlayerState
            {
                PositionX = player.CurrentPosition.X,
                PositionY = player.CurrentPosition.Y,
                Direction = player.CurrentFacingDirection.DisplayName
            }
        };
    }
}

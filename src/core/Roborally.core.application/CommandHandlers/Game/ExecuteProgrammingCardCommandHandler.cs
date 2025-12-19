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

public class ExecuteProgrammingCardCommandHandler : ICommandHandler<ExecuteProgrammingCardCommand>
{
    private readonly IGameRepository _gameRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGameBroadcaster _gameBroadcaster;
    private readonly ISystemTime _systemTime;

/// <author name="Suhani Pandey 2025-10-15 21:47:56 +0200 22" />
    public ExecuteProgrammingCardCommandHandler(IGameRepository gameRepository, IUnitOfWork unitOfWork, IGameBroadcaster gameBroadcaster, ISystemTime systemTime)
    {
        _gameRepository = gameRepository;
        _unitOfWork = unitOfWork;
        _gameBroadcaster = gameBroadcaster;
        _systemTime = systemTime;
    }

/// <author name="Nilanjana Devkota 2025-11-13 19:56:43 +0100 30" />
    public async Task ExecuteAsync(ExecuteProgrammingCardCommand command, CancellationToken ct)
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
        
        // Build optional context for interactive cards
        var executionContext = BuildExecutionContext(command);
        
        // Get count of checkpoint events before execution

        // Execute card and get all affected players (executor + pushed robots)
        List<Player> affectedPlayers = game.ExecuteProgrammingCard(command.Username, card, _systemTime, executionContext);
        

        
        await _unitOfWork.SaveChangesAsync(ct);
        
        // Broadcast ALL affected players (executor + pushed robots)
        foreach (var player in affectedPlayers)
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

        Player? nextPlayer = game.GetNextExecutingPlayer();
        await _gameBroadcaster.BroadcastNextPlayerInTurn(command.GameId, nextPlayer?.Username, ct);
    }

/// <author name="Satish Gurung 2025-11-24 10:20:04 +0100 98" />
    private static CardExecutionContext? BuildExecutionContext(ExecuteProgrammingCardCommand command)
    {
        if (command.InteractiveInput is null)
        {
            return null;
        }

        ProgrammingCard? selectedMovementCard = null;

        if (!string.IsNullOrWhiteSpace(command.InteractiveInput.SelectedMovementCard))
        {
            try
            {
                selectedMovementCard = ProgrammingCard.FromString(command.InteractiveInput.SelectedMovementCard);
            }
            catch (ArgumentException)
            {
                throw new CustomException($"Invalid selected movement card: {command.InteractiveInput.SelectedMovementCard}", 400);
            }
        }

        return new CardExecutionContext
        {
            TargetPlayerUsername = string.IsNullOrWhiteSpace(command.InteractiveInput.TargetPlayerUsername)
                ? null
                : command.InteractiveInput.TargetPlayerUsername,
            SelectedMovementCard = selectedMovementCard
        };
    }
}
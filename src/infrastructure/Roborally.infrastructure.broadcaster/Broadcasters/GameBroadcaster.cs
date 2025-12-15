using Microsoft.AspNetCore.SignalR;
using Roborally.core.application.ApplicationContracts.Broadcasters;
using Roborally.core.domain.BroadCastEvents;
using Roborally.core.domain.Game;
using Roborally.core.domain.Game.Deck;
using Roborally.infrastructure.broadcaster.Game;

namespace Roborally.infrastructure.broadcaster.Broadcasters;

public class GameBroadcaster : IGameBroadcaster{

    private readonly IHubContext<GameHub> _hubContext;

    private static string GroupName(Guid gameId) => $"game-{gameId.ToString()}";
    
    public GameBroadcaster(IHubContext<GameHub> hubContext) {
        this._hubContext = hubContext;
    }


    public Task BroadcastPlayerLockedInRegisterAsync(string username, Guid gameId, CancellationToken ct) {
        var payload = new {
            username,
        };
        return _hubContext.Clients.Groups(GroupName(gameId)).SendAsync("PlayerLockedInRegister", payload, ct);
    }

    public Task BroadcastActivationPhaseStartedAsync(Guid gameId, CancellationToken ct)
    {
        var payload = new {
            gameId,
        };
        return _hubContext.Clients.Groups(GroupName(gameId)).SendAsync("ActivationPhaseStarted", payload, ct);
    }

    public Task BroadcastRegisterRevealedAsync(Guid gameId, int registerNumber, Dictionary<string, ProgrammingCard> revealedCards, CancellationToken ct)
    {
        var payload = new
        {
            gameId,
            registerNumber,
            revealedCards = revealedCards.Select(kvp => new
            {
                username = kvp.Key,
                card = kvp.Value.DisplayName
            }).ToList()
        };
        return _hubContext.Clients.Groups(GroupName(gameId)).SendAsync("RegisterRevealed", payload, ct);
    }

    public Task BroadcastRobotMovedAsync(Guid gameId, string username, int positionX, int positionY, string direction, string executedCard, CancellationToken ct)
    {
        var payload = new
        {
            gameId,
            username,
            positionX,
            positionY,
            direction,
            executedCard
        };
        return _hubContext.Clients.Groups(GroupName(gameId)).SendAsync("RobotMoved", payload, ct);
    }
    
    public Task BroadcastPauseGameRequestedAsync(Guid gameId, string requesterUsername, CancellationToken ct)
    {
        var payload = new
        {
            gameId,
            requesterUsername
        };
        return _hubContext.Clients.Groups(GroupName(gameId)).SendAsync("GamePauseRequested", payload, ct);
    }
    
    public Task BroadcastPauseGameResultAsync(Guid gameId, GamePauseState state, CancellationToken ct)
    {
        var payload = new
        {
            gameId,
            result = state.result,
            requestedBy = state.RequestedBy,
            playerResponses = state.PlayerResponses,
            requestedAt = state.RequestedAt
        };
        return _hubContext.Clients.Groups(GroupName(gameId)).SendAsync("GamePauseResult", payload, ct);
    }

    public Task BroadcastNextPlayerInTurn(Guid gameId, string? nextPlayerUsername, CancellationToken ct) {
        var payload = new
        {
            gameId,
            nextPlayerUsername
        };
        return _hubContext.Clients.Groups(GroupName(gameId)).SendAsync("NextPlayerInTurn", payload, ct);
    }

    public Task BroadcastCheckpointReachedAsync(Guid gameId, string username, int checkpointNumber, CancellationToken ct)
    {
        var payload = new
        {
            gameId,
            username,
            checkpointNumber
        };
        return _hubContext.Clients.Groups(GroupName(gameId)).SendAsync("CheckpointReached", payload, ct);
    }

    public async Task BroadcastGameCompletedAsync(GameCompletedBroadcastEvent eventModel,CancellationToken ct)
    {

        await _hubContext.Clients.Groups(GroupName(eventModel.GameId)).SendAsync("GameCompleted", eventModel, ct);
    }

    public Task BroadcastRoundCompletedAsync(Guid gameId, int completedRound, int newRound, CancellationToken ct)
    {
        var payload = new
        {
            gameId,
            completedRound,
            newRound
        };
        return _hubContext.Clients.Groups(GroupName(gameId)).SendAsync("RoundCompleted", payload, ct);
    }
    
    public Task BroadcastProgrammingTimeoutAsync(Guid gameId, Dictionary<string, List<ProgrammingCard>> assignedCards, CancellationToken ct)
    {
        var payload = new
        {
            gameId,
            assignedCards
        };
        return _hubContext.Clients.Groups(GroupName(gameId)).SendAsync("ProgrammingTimeout", payload, ct);
    }
}
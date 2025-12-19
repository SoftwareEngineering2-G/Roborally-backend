using Microsoft.AspNetCore.SignalR;
using Roborally.core.application.ApplicationContracts.Broadcasters;
using Roborally.core.domain.BroadCastEvents;
using Roborally.core.domain.Game;
using Roborally.core.domain.Game.Deck;
using Roborally.infrastructure.broadcaster.Game;

namespace Roborally.infrastructure.broadcaster.Broadcasters;

public class GameBroadcaster : IGameBroadcaster{

    private readonly IHubContext<GameHub> _hubContext;

/// <author name="Sachin Baral 2025-09-28 13:55:14 +0200 14" />
    private static string GroupName(Guid gameId) => $"game-{gameId.ToString()}";
    
/// <author name="Sachin Baral 2025-10-01 21:53:45 +0200 16" />
    public GameBroadcaster(IHubContext<GameHub> hubContext) {
        this._hubContext = hubContext;
    }


/// <author name="Vincenzo Altaserse 2025-12-18 17:40:31 +0100 21" />
    public Task BroadcastPlayerLockedInRegisterAsync(string username, string? timeoutExpiresAt, Guid gameId, CancellationToken ct) {
        var payload = new {
            username,
            timeoutExpiresAt
        };
        return _hubContext.Clients.Groups(GroupName(gameId)).SendAsync("PlayerLockedInRegister", payload, ct);
    }

/// <author name="Suhani Pandey 2025-10-10 13:01:53 +0200 29" />
    public Task BroadcastActivationPhaseStartedAsync(Guid gameId, CancellationToken ct)
    {
        var payload = new {
            gameId,
        };
        return _hubContext.Clients.Groups(GroupName(gameId)).SendAsync("ActivationPhaseStarted", payload, ct);
    }

/// <author name="Suhani Pandey 2025-10-10 13:01:53 +0200 37" />
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

/// <author name="Suhani Pandey 2025-10-10 13:01:53 +0200 52" />
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
    
/// <author name="Truong Son NGO 2025-11-12 15:35:28 +0100 66" />
    public Task BroadcastPauseGameRequestedAsync(Guid gameId, string requesterUsername, CancellationToken ct)
    {
        var payload = new
        {
            gameId,
            requesterUsername
        };
        return _hubContext.Clients.Groups(GroupName(gameId)).SendAsync("GamePauseRequested", payload, ct);
    }
    
/// <author name="Truong Son NGO 2025-11-12 15:35:28 +0100 76" />
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

/// <author name="Truong Son NGO 2025-11-12 15:35:28 +0100 89" />
    public Task BroadcastNextPlayerInTurn(Guid gameId, string? nextPlayerUsername, CancellationToken ct) {
        var payload = new
        {
            gameId,
            nextPlayerUsername
        };
        return _hubContext.Clients.Groups(GroupName(gameId)).SendAsync("NextPlayerInTurn", payload, ct);
    }

/// <author name="Suhani Pandey 2025-11-13 16:20:36 +0100 98" />
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

/// <author name="Sachin Baral 2025-11-15 19:46:26 +0100 109" />
    public async Task BroadcastGameCompletedAsync(GameCompletedBroadcastEvent eventModel,CancellationToken ct)
    {

        await _hubContext.Clients.Groups(GroupName(eventModel.GameId)).SendAsync("GameCompleted", eventModel, ct);
    }

/// <author name="Suhani Pandey 2025-12-03 21:46:28 +0100 115" />
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
}
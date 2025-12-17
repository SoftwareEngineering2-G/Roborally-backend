using Roborally.core.domain.BroadCastEvents;
using Roborally.core.domain.Game;
using Roborally.core.domain.Game.Deck;

namespace Roborally.core.application.ApplicationContracts.Broadcasters;

public interface IGameBroadcaster {
    Task BroadcastPlayerLockedInRegisterAsync(string username, string? timeoutExpiresAt, Guid gameId, CancellationToken ct);
    Task BroadcastActivationPhaseStartedAsync(Guid gameId, CancellationToken ct);
    Task BroadcastRegisterRevealedAsync(Guid gameId, int registerNumber, Dictionary<string, ProgrammingCard> revealedCards, CancellationToken ct);
    Task BroadcastRobotMovedAsync(Guid gameId, string username, int positionX, int positionY, string direction, string executedCard, CancellationToken ct);
    Task BroadcastPauseGameRequestedAsync(Guid gameId, string requesterUsername, CancellationToken ct);
    Task BroadcastPauseGameResultAsync(Guid gameId, GamePauseState state, CancellationToken ct);
    Task BroadcastNextPlayerInTurn(Guid gameId, string? nextPlayerUsername, CancellationToken ct);
    Task BroadcastGameCompletedAsync(GameCompletedBroadcastEvent eventModel, CancellationToken ct);
    Task BroadcastCheckpointReachedAsync(Guid gameId, string username, int checkpointNumber, CancellationToken ct);
    Task BroadcastRoundCompletedAsync(Guid gameId, int completedRound, int newRound, CancellationToken ct);
    Task BroadcastProgrammingTimeoutAsync(Guid gameId, Dictionary<string, List<ProgrammingCard>> assignedCards, CancellationToken ct);
}
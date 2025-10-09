using Roborally.core.domain.Deck;

namespace Roborally.core.application.Broadcasters;

public interface IGameBroadcaster {
    Task BroadcastPlayerLockedInRegisterAsync(string username, Guid gameId, CancellationToken ct);
    Task BroadcastActivationPhaseStartedAsync(Guid gameId, CancellationToken ct);
    Task BroadcastRegisterRevealedAsync(Guid gameId, int registerNumber, Dictionary<string, ProgrammingCard> revealedCards, CancellationToken ct);
}
﻿using Roborally.core.domain.Game.Deck;

namespace Roborally.core.application.Broadcasters;

public interface IGameBroadcaster {
    Task BroadcastPlayerLockedInRegisterAsync(string username, Guid gameId, CancellationToken ct);
    Task BroadcastActivationPhaseStartedAsync(Guid gameId, CancellationToken ct);
    Task BroadcastRegisterRevealedAsync(Guid gameId, int registerNumber, Dictionary<string, ProgrammingCard> revealedCards, CancellationToken ct);
    Task BroadcastRobotMovedAsync(Guid gameId, string username, int positionX, int positionY, string direction, string executedCard, CancellationToken ct);
}
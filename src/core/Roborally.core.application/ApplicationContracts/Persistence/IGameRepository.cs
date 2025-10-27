﻿using Roborally.core.application.QueryContracts;
using Roborally.core.domain.Game;

namespace Roborally.core.application.ApplicationContracts.Persistence;

public interface IGameRepository {

    Task AddAsync(Game game, CancellationToken ct);
    Task<Game?> FindAsync(Guid gameId, CancellationToken ct);


    Task<List<GetGamesForUserResponse>> QueryGamesForUserAsync(GetGamesForUserQuery query, CancellationToken ct);
    Task<GetCurrentUserPlayingStatusResponse> QueryUserCurrentPlayingStatusAsync(GetUserCurrentPlayingStatusQuery command, CancellationToken ct);
}
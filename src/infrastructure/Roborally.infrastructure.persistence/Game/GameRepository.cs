using Microsoft.EntityFrameworkCore;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.QueryContracts;

namespace Roborally.infrastructure.persistence.Game;

public class GameRepository : IGameRepository {
    private readonly AppDatabaseContext _context;

    public GameRepository(AppDatabaseContext context) {
        _context = context;
    }

    public Task AddAsync(core.domain.Game.Game game, CancellationToken ct) {
        return _context.Games.AddAsync(game, ct).AsTask();
    }

    public Task<core.domain.Game.Game?> FindAsync(Guid gameId, CancellationToken ct) {
        return _context.Games.Include(game => game.Players)
            .ThenInclude(player => player.PlayerEvents)
            .Include(game => game.GameBoard)
            .FirstOrDefaultAsync(game => game.GameId.Equals(gameId), ct);
    }

    public async Task<List<GetGamesForUserResponse>>
        QueryGamesForUserAsync(GetGamesForUserQuery query, CancellationToken ct) {
        // The queries dont need to be tracked by EF Core as we are only reading data
        var queryable =
            _context.Games.Where(game => game.Players.Select(player => player.Username).Contains(query.Username))
                .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.SearchTag)) {
            query.SearchTag = query.SearchTag.Trim().ToLower();
            queryable = queryable.Where(game => game.Name.ToLower().Contains(query.SearchTag) ||
                                                game.HostUsername.ToLower().Contains(query.SearchTag));
        }

        if (query.IsFinished.HasValue && query.IsFinished.Value) {
            queryable = queryable.Where(game => game.CompletedAt != null);
        }

        if (query.IsPrivate.HasValue && query.IsPrivate.Value) {
            queryable = queryable.Where(game => game.IsPrivate);
        }

        if (query.From is not null) {
            queryable = queryable.Where(game => DateOnly.FromDateTime(game.CreatedAt) >= query.From);
        }

        if (query.To is not null) {
            queryable = queryable.Where(game => DateOnly.FromDateTime(game.CreatedAt) <= query.To);
        }

        return await queryable.OrderByDescending(game => game.CreatedAt)
            .Select(game => new GetGamesForUserResponse {
                GameId = game.GameId,
                GameRoomName = game.Name,
                HostUsername = game.HostUsername,
                StartDate = DateOnly.FromDateTime(game.CreatedAt),
                IsFinished = game.CompletedAt != null,
                IsPrivate = game.IsPrivate,
            })
            .ToListAsync(ct); // Only one database call because we only call ToListAsync once
    }

    public async Task<GetCurrentUserPlayingStatusResponse> QueryUserCurrentPlayingStatusAsync(
        GetUserCurrentPlayingStatusQuery query, CancellationToken ct) {
        var game =
            await _context.Games
                .Where(game => game.Players.Select(player => player.Username).Contains(query.Username))
                .Where(game => game.CompletedAt == null)
                .AsNoTracking()
                .FirstOrDefaultAsync(ct);

        // If on a game, directly return the game...
        if (game is not null) {
            return new GetCurrentUserPlayingStatusResponse() {
                IsCurrentlyOnAGame = true,
                IsCurrentlyOnAGameLobby = false,
                GameId = game.GameId
            };
        }

        var lobby = await _context.GameLobby
            .Include(lobby => lobby.JoinedUsers)
            .Where(lobby => lobby.JoinedUsers.Select(user => user.Username).Contains(query.Username))
            .AsNoTracking()
            .FirstOrDefaultAsync(ct);

        if (lobby is not null) {
            return new GetCurrentUserPlayingStatusResponse() {
                IsCurrentlyOnAGame = false,
                IsCurrentlyOnAGameLobby = true,
                GameId = lobby.GameId
            };
        }

        return new GetCurrentUserPlayingStatusResponse() {
            IsCurrentlyOnAGame = false,
            IsCurrentlyOnAGameLobby = false,
            GameId = null
        };
    }
}
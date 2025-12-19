using Microsoft.EntityFrameworkCore;
using Roborally.core.application;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.QueryContracts;
using Roborally.core.domain.User;

namespace Roborally.infrastructure.persistence.User;

public class UserRepository : IUserRepository {
    private readonly AppDatabaseContext _context;

/// <author name="Sachin Baral 2025-09-14 12:59:43 +0200 12" />
    public UserRepository(AppDatabaseContext context) {
        _context = context;
    }

/// <author name="Sachin Baral 2025-09-14 12:59:43 +0200 16" />
    public Task AddAsync(core.domain.User.User user, CancellationToken cancellationToken = default) {
        return _context.Users.AddAsync(user, cancellationToken).AsTask();
    }


/// <author name="Sachin Baral 2025-09-19 13:01:07 +0200 21" />
    public Task<core.domain.User.User?> FindAsync(string username, CancellationToken cancellationToken = default) {
        return _context.Users.FirstOrDefaultAsync(u => u.Username.ToLower().Equals(username.ToLower()), cancellationToken);
    }

/// <author name="Sachin Baral 2025-09-16 11:52:08 +0200 25" />
    public Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken = default) {
        return _context.Users.AnyAsync(u => u.Username.ToLower().Equals(username.ToLower()), cancellationToken);
    }

/// <author name="Sachin Baral 2025-11-15 19:46:26 +0100 29" />
    public async Task<GetLeaderboardQueryResult> GetLeaderboardQueryAsync(GetLeaderboardQuery query, CancellationToken ct) {
        var totalCount = await _context.Users.CountAsync(ct);

        var items = await _context.Users.AsNoTracking()
            .OrderByDescending(u => u.Rating)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(u => new GetLeaderboardQueryResponse {
                Username = u.Username,
                Rating = u.Rating
            })
            .ToListAsync(ct);

        var totalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize);

        return new GetLeaderboardQueryResult {
            Items = items,
            TotalCount = totalCount,
            TotalPages = totalPages,
            CurrentPage = query.PageNumber,
            PageSize = query.PageSize
        };
    }
}
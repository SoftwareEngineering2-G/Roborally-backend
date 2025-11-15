using Microsoft.EntityFrameworkCore;
using Roborally.core.application;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.QueryContracts;
using Roborally.core.domain.User;

namespace Roborally.infrastructure.persistence.User;

public class UserRepository : IUserRepository {
    private readonly AppDatabaseContext _context;

    public UserRepository(AppDatabaseContext context) {
        _context = context;
    }

    public Task AddAsync(core.domain.User.User user, CancellationToken cancellationToken = default) {
        return _context.Users.AddAsync(user, cancellationToken).AsTask();
    }


    public Task<core.domain.User.User?> FindAsync(string username, CancellationToken cancellationToken = default) {
        return _context.Users.FirstOrDefaultAsync(u => u.Username.ToLower().Equals(username.ToLower()), cancellationToken);
    }

    public Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken = default) {
        return _context.Users.AnyAsync(u => u.Username.ToLower().Equals(username.ToLower()), cancellationToken);
    }

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
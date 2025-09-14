using Microsoft.EntityFrameworkCore;
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

    public Task<core.domain.User.User?> FindAsync(Guid userId) {
        return _context.Users.FindAsync(userId).AsTask();
    }

    public Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken = default) {
        return _context.Users.AnyAsync(u => u.Username.Equals(username), cancellationToken);
    }
}
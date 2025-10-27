using Roborally.core.domain.User;

namespace Roborally.core.application.ApplicationContracts.Persistence;

public interface IUserRepository {

    Task AddAsync(User user, CancellationToken cancellationToken = default);
    Task<User?> FindAsync(string username, CancellationToken cancellationToken = default);
    Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken = default);
    
}
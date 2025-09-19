namespace Roborally.core.domain.User;

public interface IUserRepository {

    Task AddAsync(User user, CancellationToken cancellationToken = default);
    Task<User?> FindAsync(string username, CancellationToken cancellationToken = default);
    Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken = default);
    
}
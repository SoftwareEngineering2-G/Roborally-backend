
namespace Roborally.core.domain.User;

public interface IUserRepository {

    Task AddAsync(User user, CancellationToken cancellationToken = default);
    Task<User?> FindAsync(Guid userId);
    
}
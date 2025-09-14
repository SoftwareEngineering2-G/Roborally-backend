/**
 * Author: Sachin (s243871)
 * Date: September 14, 2025
 * Description: Unit of Work interface for managing database transactions
 */

namespace Roborally.core.domain.Bases;

public interface IUnitOfWork {
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
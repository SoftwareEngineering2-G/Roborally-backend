using Roborally.core.domain.Bases;

namespace Roborally.infrastructure.persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDatabaseContext _context;

    public UnitOfWork(AppDatabaseContext context)
    {
        _context = context;
    }


    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
using Roborally.core.domain.Bases;

namespace Roborally.infrastructure.persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDatabaseContext _context;

/// <author name="Truong Son NGO 2025-09-19 17:04:26 +0200 9" />
    public UnitOfWork(AppDatabaseContext context)
    {
        _context = context;
    }


/// <author name="Truong Son NGO 2025-09-19 17:04:26 +0200 15" />
    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
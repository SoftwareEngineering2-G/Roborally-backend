using Microsoft.EntityFrameworkCore;

namespace Roborally.infrastructure.persistence;

public class AppDatabaseContext: DbContext{

    public AppDatabaseContext(DbContextOptions<AppDatabaseContext> options) : base(options){}

    public required DbSet<core.domain.User.User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDatabaseContext).Assembly);
    }
}
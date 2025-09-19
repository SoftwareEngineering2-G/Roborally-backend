using Microsoft.EntityFrameworkCore;
using FastEndpoints;
using Roborally.core.domain.Bases;

namespace Roborally.infrastructure.persistence;

public class AppDatabaseContext: DbContext{

    public AppDatabaseContext(DbContextOptions<AppDatabaseContext> options) : base(options){}

    public required DbSet<core.domain.User.User> Users { get; set; }
    public required DbSet<core.domain.Lobby.GameLobby> GameLobby { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDatabaseContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Collect all domain events from tracked entities before saving
        var domainEvents = ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any())
            .SelectMany(x => x.Entity.DomainEvents!)
            .ToList();

        // Publish domain events first
        foreach (var domainEvent in domainEvents)
        {
            await domainEvent.PublishAsync(cancellation: cancellationToken);
        }

        // Save changes to database after successful event publishing
        var result = await base.SaveChangesAsync(cancellationToken);

        // Clear domain events from entities after both publishing and saving
        foreach (var entry in ChangeTracker.Entries<Entity>())
        {
            entry.Entity.ClearDomainEvents();
        }

        return result;
    }
}
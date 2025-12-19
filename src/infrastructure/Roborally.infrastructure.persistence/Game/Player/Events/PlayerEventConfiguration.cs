using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roborally.core.domain.Game.Player.Events;

namespace Roborally.infrastructure.persistence.Game.Player.Events;

public class PlayerEventConfiguration : IEntityTypeConfiguration<PlayerEvent>
{
/// <author name="Sachin Baral 2025-09-28 13:55:14 +0200 9" />
    public void Configure(EntityTypeBuilder<PlayerEvent> builder)
    {
        builder.ToTable("PlayerEvents");
        
        // Primary key from base Event class
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedOnAdd();
        
        // Common columns for all events
        builder.Property(e => e.GameId).IsRequired();
        builder.Property(e => e.Username).IsRequired();
        builder.Property(e => e.HappenedAt).IsRequired();
        
        builder.UseTptMappingStrategy();
    }
}



using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roborally.core.domain.Game.GameEvents;

namespace Roborally.infrastructure.persistence.Game.Events;

public class GameEventConfiguration : IEntityTypeConfiguration<GameEvent> {



/// <author name="Nilanjana Devkota 2025-10-14 19:37:00 +0200 11" />
    public void Configure(EntityTypeBuilder<GameEvent> builder) {
        builder.ToTable("GameEvents");
        
        // Primary key from base Event class
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedOnAdd();
        
        // Common columns for all events
        builder.Property(e => e.GameId).IsRequired();
        builder.Property(e => e.HappenedAt).IsRequired();
        
        builder.UseTptMappingStrategy();
    }
}
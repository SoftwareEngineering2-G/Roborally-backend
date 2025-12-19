using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roborally.core.domain.Game.GameEvents;

namespace Roborally.infrastructure.persistence.Game.Events;

public class CheckpointReachedEventConfiguration : IEntityTypeConfiguration<CheckpointReachedEvent> {
/// <author name="Suhani Pandey 2025-11-13 16:20:36 +0100 8" />
    public void Configure(EntityTypeBuilder<CheckpointReachedEvent> builder) {
        builder.ToTable("CheckpointReachedEvents");
    }
}

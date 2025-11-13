using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roborally.core.domain.Game.GameEvents;

namespace Roborally.infrastructure.persistence.Game.Events;

public class CheckpointReachedEventConfiguration : IEntityTypeConfiguration<CheckpointReachedEvent> {
    public void Configure(EntityTypeBuilder<CheckpointReachedEvent> builder) {
        builder.ToTable("CheckpointReachedEvents");
    }
}


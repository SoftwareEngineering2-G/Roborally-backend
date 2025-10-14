using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roborally.core.domain.Game.GameEvents;

namespace Roborally.infrastructure.persistence.Game.Events;

public class BoardElementActivatedEventConfiguration : IEntityTypeConfiguration<BoardElementActivatedEvent> {
    public void Configure(EntityTypeBuilder<BoardElementActivatedEvent> builder) {
        builder.ToTable("BoardElementActivatedEvents");
    }
}
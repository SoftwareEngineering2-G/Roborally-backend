using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roborally.core.domain.Game.GameEvents;

namespace Roborally.infrastructure.persistence.Game.Events;

public class BoardElementActivatedEventConfiguration : IEntityTypeConfiguration<BoardElementActivatedEvent> {
/// <author name="nilanjanadevkota 2025-10-14 19:37:00 +0200 8" />
    public void Configure(EntityTypeBuilder<BoardElementActivatedEvent> builder) {
        builder.ToTable("BoardElementActivatedEvents");
    }
}
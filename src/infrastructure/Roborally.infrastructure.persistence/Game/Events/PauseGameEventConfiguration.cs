using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roborally.core.domain.Game.GameEvents;

namespace Roborally.infrastructure.persistence.Game.Events;

public class PauseGameEventConfiguration : IEntityTypeConfiguration<PauseGameEvent> {
/// <author name="Truong Son NGO 2025-11-12 15:35:28 +0100 8" />
    public void Configure(EntityTypeBuilder<PauseGameEvent> builder) {
        builder.ToTable("PauseGameEvents");
    }
}
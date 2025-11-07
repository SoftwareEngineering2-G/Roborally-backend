using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roborally.core.domain.Game.GameEvents;

namespace Roborally.infrastructure.persistence.Game.Events;

public class PauseGameEventConfiguration : IEntityTypeConfiguration<PauseGameEvent> {
    public void Configure(EntityTypeBuilder<PauseGameEvent> builder) {
        builder.ToTable("PauseGameEvents");
    }
}
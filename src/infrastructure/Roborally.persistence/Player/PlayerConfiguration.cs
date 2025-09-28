using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;
using Roborally.core.domain.Deck;

namespace Roborally.infrastructure.persistence.Player;

public class PlayerConfiguration : IEntityTypeConfiguration<core.domain.Player.Player> {
    public void Configure(EntityTypeBuilder<core.domain.Player.Player> builder) {
        // Composite primary key..
        builder.HasKey(x => new {x.GameId, x.Username});

        builder.ComplexProperty(entity => entity.Robot,
            propBuilder => { propBuilder.Property(r => r.DisplayName).HasColumnName("Robot"); });

        builder.ComplexProperty(entity => entity.CurrentFacingDirection,
            propBuilder => { propBuilder.Property(c => c.DisplayName).HasColumnName("CurrentFacingDirection"); });

        builder.ComplexProperty(entity => entity.ProgrammingDeck, propBuilder =>
        {
            propBuilder.Property(d => d.ProgrammingCards)
                .HasConversion(
                    // Convert from ProgrammingCard list to JSON string
                    v => JsonSerializer.Serialize(
                        v.Select(card => card.DisplayName).ToList(),
                        new JsonSerializerOptions()),
                    // Convert from JSON string back to ProgrammingCard list
                    v => JsonSerializer.Deserialize<List<string>>(v, new JsonSerializerOptions())!
                        .Select(ProgrammingCard.FromString)
                        .ToList())
                .HasColumnName("ProgrammingCards");
        });
    }
}

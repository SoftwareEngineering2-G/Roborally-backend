using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Roborally.core.domain.Game.Player.Events;
using System.Text.Json;
using System.Linq;
using Roborally.core.domain.Game.Deck;

namespace Roborally.infrastructure.persistence.Game.Player.Events;

public class ProgrammingCardsDealtEventConfiguration : IEntityTypeConfiguration<ProgrammingCardsDealtEvent>
{
    public void Configure(EntityTypeBuilder<ProgrammingCardsDealtEvent> builder)
    {
        builder.ToTable("ProgrammingCardsDealtEvents");

        var compactJsonOptions = new JsonSerializerOptions
        {
            WriteIndented = false
        };

        builder.Property(e => e.DealtCards)
            .HasConversion(
                v => JsonSerializer.Serialize(v.Select(card => card.DisplayName).ToList(), compactJsonOptions),
                v => JsonSerializer.Deserialize<List<string>>(v, compactJsonOptions)!.Select(ProgrammingCard.FromString).ToList(),
                new ValueComparer<List<ProgrammingCard>>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()))
            .HasColumnType("json");
    }
}

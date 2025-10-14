using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roborally.core.domain.Game.Deck;
using Roborally.core.domain.Game.Player.Events;

namespace Roborally.infrastructure.persistence.Game.Player.Events;

public class  RegistersProgrammedEventConfiguration : IEntityTypeConfiguration<RegistersProgrammedEvent>
{
    public void Configure(EntityTypeBuilder<RegistersProgrammedEvent> builder)
    {
        builder.ToTable("RegisterProgrammedEvents");
        
        var compactJsonOptions = new JsonSerializerOptions
        {
            WriteIndented = false
        };
        
        builder.Property(e => e.ProgrammedCardsInOrder)
            .HasConversion(
                // Convert from ProgrammingCard list to JSON string (order preserved)
                v => JsonSerializer.Serialize(
                    v.Select(card => card.DisplayName).ToList(),
                    compactJsonOptions),
                // Convert from JSON string back to ProgrammingCard list (order preserved)  
                v => JsonSerializer.Deserialize<List<string>>(v, compactJsonOptions)!
                    .Select(ProgrammingCard.FromString)
                    .ToList(),
                // Value comparer for proper change detection
                new ValueComparer<List<ProgrammingCard>>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()))
            .HasColumnName("ProgrammedCardsInOrder")
            .HasColumnType("json");
    }
}
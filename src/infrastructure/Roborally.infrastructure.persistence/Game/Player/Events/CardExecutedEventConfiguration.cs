using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roborally.core.domain.Game.Player.Events;

namespace Roborally.infrastructure.persistence.Game.Player.Events;

public class CardExecutedEventConfiguration : IEntityTypeConfiguration<CardExecutedEvent>
{
/// <author name="Suhani Pandey 2025-10-15 21:47:56 +0200 9" />
    public void Configure(EntityTypeBuilder<CardExecutedEvent> builder)
    {
        builder.ToTable("CardExecutedEvents");
        
        // Configure the Card property to store the card's DisplayName
        builder.ComplexProperty(e => e.Card, propBuilder =>
        {
            propBuilder.Property(c => c.DisplayName)
                .HasColumnName("CardName")
                .HasMaxLength(50)
                .IsRequired();
        });
    }
}
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roborally.core.domain.Game.Deck;

namespace Roborally.infrastructure.persistence.Game.Player;

public class PlayerConfiguration : IEntityTypeConfiguration<core.domain.Game.Player.Player> {
    public void Configure(EntityTypeBuilder<core.domain.Game.Player.Player> builder) {
        // Composite primary key (Username, GameId)
        builder.HasKey(x => new {x.GameId, x.Username});
        
        builder.ToTable("Players");


        // Configure foreign key relationships
        // Username references Users table
        builder.HasOne<core.domain.User.User>()
            .WithMany() // User doesn't have navigation property to Players
            .HasForeignKey(p => p.Username)
            .HasPrincipalKey(u => u.Username)
            .OnDelete(DeleteBehavior.Restrict) // Prevent deletion of User if they have Players
            .IsRequired();

        // GameId references Games table  
        builder.HasOne<core.domain.Game.Game>()
            .WithMany(g => g.Players) // Game has navigation property to Players
            .HasForeignKey(p => p.GameId)
            .HasPrincipalKey(g => g.GameId)
            .OnDelete(DeleteBehavior.Cascade) // Delete Players when Game is deleted
            .IsRequired();

        // Configure the relationship with PlayerEvents
        builder.HasMany(p => p.PlayerEvents)
            .WithOne() // PlayerEvent doesn't have a navigation property back to Player
            .HasForeignKey(pe => new { pe.GameId, pe.Username }) // Use composite foreign key
            .IsRequired();

        builder.ComplexProperty(entity => entity.Robot,
            propBuilder => { propBuilder.Property(r => r.DisplayName).HasColumnName("Robot"); });

        builder.ComplexProperty(entity => entity.CurrentFacingDirection,
            propBuilder => { propBuilder.Property(c => c.DisplayName).HasColumnName("CurrentFacingDirection"); });

        builder.ComplexProperty(entity => entity.CurrentPosition, propBuilder =>
        {
            propBuilder.Property(p => p.X).HasColumnName("CurrentPositionX");
            propBuilder.Property(p => p.Y).HasColumnName("CurrentPositionY");
        });

        builder.ComplexProperty(entity => entity.SpawnPosition, propBuilder =>
        {
            propBuilder.Property(p => p.X).HasColumnName("SpawnPositionX");
            propBuilder.Property(p => p.Y).HasColumnName("SpawnPositionY");
        });


        // We can just save the deck of the player as a list of card names in order.
        builder.ComplexProperty(entity => entity.ProgrammingDeck, propBuilder =>
        {
            var compactJsonOptions = new JsonSerializerOptions
            {
                WriteIndented = false // Compact JSON without spaces/indentation
            };

            propBuilder.Property(d => d.PickPiles)
                .HasConversion(
                    v => JsonSerializer.Serialize(
                        v.Select(card => card.DisplayName).ToList(),
                        compactJsonOptions),

                    v => JsonSerializer.Deserialize<List<string>>(v, compactJsonOptions)!
                        .Select(ProgrammingCard.FromString)
                        .ToList(),
                    // Value comparer for proper change detection
                    new ValueComparer<List<ProgrammingCard>>(
                        (c1, c2) => c1!.SequenceEqual(c2!),
                        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                        c => c.ToList()))
                .HasColumnName("PickPiles")
                .HasColumnType("json"); // Explicitly specify JSON column type
                
            propBuilder.Property(d => d.DiscardedPiles)
                .HasConversion(
                    v => JsonSerializer.Serialize(
                        v.Select(card => card.DisplayName).ToList(),
                        compactJsonOptions),

                    v => JsonSerializer.Deserialize<List<string>>(v, compactJsonOptions)!
                        .Select(ProgrammingCard.FromString)
                        .ToList(),
                    // Value comparer for proper change detection
                    new ValueComparer<List<ProgrammingCard>>(
                        (c1, c2) => c1!.SequenceEqual(c2!),
                        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                        c => c.ToList()))
                .HasColumnName("DiscardedPiles")
                .HasColumnType("json"); // Explicitly specify JSON column type
        });
    }
}
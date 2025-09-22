using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Roborally.core.domain.Game;

namespace Roborally.infrastructure.persistence.Player;

public class PlayerConfiguration:IEntityTypeConfiguration<core.domain.Game.Player>
{
    public void Configure(EntityTypeBuilder<core.domain.Game.Player> builder)
    {
        builder.ToTable("Players");
        builder.HasKey(p => p.Id);
        
        //player has one user
        builder.HasOne(p => p.User)
            .WithMany()
            .HasForeignKey("UserId")
            .IsRequired();
        
        // --- Robot ---
        var robotConverter = new ValueConverter<Robot, string>(
            v => v.DisplayName,     // to DB
            v => Robot.From(v)      // from DB
        );

        var robotComparer = new ValueComparer<Robot>(
            (a, b) => (a == null && b == null) ||
                      (a != null && b != null && a.DisplayName == b.DisplayName),
            v => v == null ? 0 : v.DisplayName.GetHashCode(),
            v => v // immutable singletons -> return as-is
        );

        builder.Property(p => p.Robot)
            .HasConversion(robotConverter)
            .HasMaxLength(32)
            .Metadata.SetValueComparer(robotComparer);

        // --- Direction ---
        var directionConverter = new ValueConverter<Direction, string>(
            v => v.DisplayName,     // to DB
            v => Direction.From(v)  // from DB
        );

        var directionComparer = new ValueComparer<Direction>(
            (a, b) => (a == null && b == null) ||
                      (a != null && b != null && a.DisplayName == b.DisplayName),
            v => v == null ? 0 : v.DisplayName.GetHashCode(),
            v => v
        );

        builder.Property(p => p.CurrentFacingDirection)
            .HasConversion(directionConverter)
            .HasMaxLength(32)
            .Metadata.SetValueComparer(directionComparer);
        
        //player has one current position which is x and y int
        builder.OwnsOne(p => p.CurrentPosition, pos =>
        {
            pos.Property(p => p.X).IsRequired();
            pos.Property(p => p.Y).IsRequired();
        });
        
        //TODO implement deck part
        builder.Ignore(p => p.ProgrammingDeck);
        
        
    }
}
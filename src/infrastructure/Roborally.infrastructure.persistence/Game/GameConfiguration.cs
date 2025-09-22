using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Roborally.core.domain.Game;

namespace Roborally.infrastructure.persistence.Game;

public class GameConfiguration : IEntityTypeConfiguration<core.domain.Game.Game>
{
    public void Configure(EntityTypeBuilder<core.domain.Game.Game> builder)
    {
        builder.HasKey(x => x.Id);
        
        //has one gameboard and gameboard has many games
        builder.HasOne(x => x.GameBoard)
            .WithMany()
            .HasForeignKey("GameBoardId")
            .OnDelete(DeleteBehavior.Restrict);
        
        
        //gamephase enumeration conversion
        var phaseConverter = new ValueConverter<GamePhase, string>(
            v => v.DisplayName,
            v => GamePhase.From(v)
        );

        var phaseComparer = new ValueComparer<GamePhase>(
            (a, b) => (a == null && b == null) || (a != null && b != null && a.DisplayName == b.DisplayName),
            v => v == null ? 0 : v.DisplayName.GetHashCode(),
            v => v
        );

        builder.Property(x => x.GamePhase)
            .HasConversion(phaseConverter)
            .HasMaxLength(32)
            .Metadata.SetValueComparer(phaseComparer);

        
        //has many players and players has many games
        builder.HasMany(x => x.Players)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "GamePlayer",
                j => j
                    .HasOne<core.domain.Game.Player>()
                    .WithMany()
                    .HasForeignKey("PlayerId")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<core.domain.Game.Game>()
                    .WithMany()
                    .HasForeignKey("GameId")
                    .OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.HasKey("GameId", "PlayerId");
                    j.ToTable("GamePlayers");
                });
        
    }
}
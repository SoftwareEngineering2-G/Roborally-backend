using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Roborally.infrastructure.persistence.Game;

public class GameConfiguration : IEntityTypeConfiguration<core.domain.Game.Game> {
    public void Configure(EntityTypeBuilder<core.domain.Game.Game> builder) {
        builder.HasKey(x => x.GameId);

        // Configure relationship with GameBoard - one GameBoard can be used by many Games
        builder.HasOne(g => g.GameBoard)
            .WithMany() // GameBoard doesn't have a navigation property back to Games
            .HasForeignKey("GameBoardName") // Use GameBoard.Name as foreign key
            .IsRequired();

        // Configure relationship with Players - one Game can have many Players
        builder.HasMany(g => g.Players)
            .WithOne() // Player doesn't have a navigation property back to Game
            .HasForeignKey(p => p.GameId) // Use Player.GameId as foreign key
            .IsRequired();

        builder.ComplexProperty(game => game.CurrentPhase,
            propBuilder => { propBuilder.Property(cp => cp.DisplayName).HasColumnName("CurrentPhase"); });
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roborally.core.domain.Lobby;

namespace Roborally.infrastructure.persistence.Lobby;

public class GameLobbyConfiguration : IEntityTypeConfiguration<GameLobby>  {
/// <author name="Suhani Pandey 2025-09-17 13:49:31 +0200 8" />
    public void Configure(EntityTypeBuilder<GameLobby> builder)
    {
        builder.HasKey(x => x.GameId);
        
        builder.Property(x => x.Name).IsRequired().HasColumnName("LobbyName");
        builder.Property(x => x.IsPrivate).IsRequired();
        builder.Property(x => x.HostUsername).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.StartedAt).IsRequired(false);
        
        // Configure HostUsername as foreign key to User table
        builder.HasOne<Roborally.core.domain.User.User>()
               .WithMany()
               .HasForeignKey(gl => gl.HostUsername)
               .OnDelete(DeleteBehavior.Restrict);
        
        // Configure many-to-many relationship for joined users
        builder.Navigation(e => e.JoinedUsers).HasField("_joinedUsers");
        builder.HasMany(gl => gl.JoinedUsers)
               .WithMany()
               .UsingEntity("GameLobbyJoinedUsers");

        // Configure many-to-many relationship for required users (nullable)
        builder.Navigation(e => e.RequiredUsers).HasField("_requiredUsers");
        builder.HasMany(gl => gl.RequiredUsers)
               .WithMany()
               .UsingEntity("GameLobbyRequiredUsers");

        builder.ToTable("GameLobby");
    }
}
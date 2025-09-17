using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roborally.core.domain.Lobby;

namespace Roborally.infrastructure.persistence.Lobby;

public class GameLobbyConfiguration : IEntityTypeConfiguration<GameLobby>  {
    public void Configure(EntityTypeBuilder<GameLobby> builder)
    {
        builder.HasKey(x => x.GameId);
        builder.Property(x => x.GameId).IsRequired();
        
        builder.Property(x => x.GameRoomName).IsRequired();
        builder.Property(x => x.IsPrivate).IsRequired();
        builder.Property(x => x.HostId).IsRequired();
        
        // TODO : replace this with one-to-many relationship when implementing join player in lobby functionality
        builder.Ignore(x => x.JoinedUsers);
        
        // one-to-one relationship since one user can only host one gamelobby at a time
        builder.HasOne<Roborally.core.domain.User.User>()
            .WithOne()
            .HasForeignKey<GameLobby>(x => x.HostId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.ToTable("GameLobby");
    }
}
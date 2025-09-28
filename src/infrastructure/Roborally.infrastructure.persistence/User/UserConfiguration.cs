using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Roborally.infrastructure.persistence.User;

public class UserConfiguration : IEntityTypeConfiguration<core.domain.User.User> {

    public void Configure(EntityTypeBuilder<core.domain.User.User> builder) {
        builder.HasKey(x => x.Username);
        
        // Configure relationship with Players - one User can have many Players (across different games)
        builder.HasMany<core.domain.Game.Player.Player>()
            .WithOne() // Player doesn't have a navigation property back to User
            .HasForeignKey(p => p.Username) // Use Player.Username as foreign key
            .IsRequired();
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Roborally.infrastructure.persistence.User;

public class UserConfiguration : IEntityTypeConfiguration<core.domain.User.User> {

    public void Configure(EntityTypeBuilder<core.domain.User.User> builder) {
        builder.HasKey(x => x.Username);
        
        // Relationship with Players is configured on the Player side (PlayerConfiguration.cs)
        // to avoid duplicate relationship configuration
    }
}
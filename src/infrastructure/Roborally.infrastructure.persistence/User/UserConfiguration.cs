using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Roborally.infrastructure.persistence.User;

public class UserConfiguration : IEntityTypeConfiguration<core.domain.User.User> {

/// <author name="Sachin Baral 2025-09-14 12:59:43 +0200 8" />
    public void Configure(EntityTypeBuilder<core.domain.User.User> builder) {
        builder.HasKey(x => x.Username);
        
        // Relationship with Players is configured on the Player side (PlayerConfiguration.cs)
        // to avoid duplicate relationship configuration
    }
}
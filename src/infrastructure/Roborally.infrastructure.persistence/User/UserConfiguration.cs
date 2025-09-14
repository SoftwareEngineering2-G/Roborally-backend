using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Roborally.infrastructure.persistence.User;

public class UserConfiguration : IEntityTypeConfiguration<core.domain.User.User> {

    public void Configure(EntityTypeBuilder<core.domain.User.User> builder) {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Username).IsUnique();
    }
}
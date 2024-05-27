using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntityConfigurations;

/// <summary>
/// Entity configuration for the User entity.
/// </summary>
/// <remarks>
/// This configuration is necessary to establish the relationships between the User entity and other entities.
/// </remarks>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .HasMany(x => x.Posts)
            .WithOne(x => x.User);

        builder
            .HasMany(x => x.Comments)
            .WithOne(x => x.User);
    }
}
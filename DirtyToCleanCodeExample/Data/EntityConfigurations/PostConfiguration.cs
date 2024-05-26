using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntityConfigurations;

/// <summary>
/// Entity configuration for the Post entity.
/// </summary>
/// <remarks>
/// This configuration is necessary to establish the relationships between the Post entity and other entities.
/// </remarks>
public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.Property(x => x.Content)
            .HasColumnType("nvarchar(MAX)");

        builder
            .HasOne(x => x.User)
            .WithMany(x => x.Posts);

        builder
            .HasMany(x => x.Comments)
            .WithOne(x => x.Post);
    }
}
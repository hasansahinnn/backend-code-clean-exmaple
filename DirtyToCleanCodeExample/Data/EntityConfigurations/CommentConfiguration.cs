using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntityConfigurations;

/// <summary>
/// Entity configuration for the Comment entity.
/// </summary>
/// <remarks>
/// This configuration is necessary to establish the relationships between the Comment entity and other entities.
/// </remarks>
public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder
            .HasOne(x => x.User)
            .WithMany(x => x.Comments);

        builder
            .HasOne(x => x.Post)
            .WithMany(x => x.Comments);
    }
}
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntityConfigurations;

/// <summary>
/// Entity configuration for the Member entity.
/// </summary>
/// <remarks>
/// This configuration is necessary to establish the relationships between the Member entity and other entities.
/// </remarks>
public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder
            .HasMany(x => x.BorrowRecords)
            .WithOne(x => x.Member);
    }
}
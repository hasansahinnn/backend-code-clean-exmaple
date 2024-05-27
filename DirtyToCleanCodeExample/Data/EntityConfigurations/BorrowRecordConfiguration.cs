using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntityConfigurations;

/// <summary>
/// Entity configuration for the BorrowRecord entity.
/// </summary>
/// <remarks>
/// This configuration is necessary to establish the relationships between the BorrowRecord entity and other entities.
/// </remarks>
public class BorrowRecordConfiguration : IEntityTypeConfiguration<BorrowRecord>
{
    public void Configure(EntityTypeBuilder<BorrowRecord> builder)
    {
        builder
            .HasOne(x => x.Book)
            .WithMany(x => x.BorrowRecords);

        builder
            .HasOne(x => x.Member)
            .WithMany(x => x.BorrowRecords);
    }
}
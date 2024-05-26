using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntityConfigurations;

/// <summary>
/// Entity configuration for the Book entity.
/// </summary>
/// <remarks>
/// This configuration is necessary to establish the relationships between the Book entity and other entities.
/// </remarks>
public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder
            .HasMany(x => x.BorrowRecords)
            .WithOne(x => x.Book);
    }
}
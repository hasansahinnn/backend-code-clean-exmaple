using Core.Data;

namespace Data.Models;

public class Book : BaseEntity
{
    public string Title { get; set; }
    public string Author { get; set; }
    public DateTime PublishedDate { get; set; }
    /// <summary>
    /// The collection of borrow records associated with the book.
    /// </summary>
    public virtual ICollection<BorrowRecord> BorrowRecords { get; set; }

    public Book()
    {
        // Required by EF Core
        BorrowRecords = new HashSet<BorrowRecord>();
    }

    public Book(int id, string title, string author, DateTime publishedDate)
    {
        BorrowRecords = new HashSet<BorrowRecord>();
    }
}
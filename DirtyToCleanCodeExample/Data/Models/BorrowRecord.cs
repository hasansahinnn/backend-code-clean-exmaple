using Core.Data;

namespace Data.Models;

public class BorrowRecord : BaseEntity
{
    public int BookId { get; set; }
    public int MemberId { get; set; }
    public DateTime BorrowDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    /// <summary>
    /// The book associated with the borrow record.
    /// </summary>
    public virtual Book Book { get; set; }
    /// <summary>
    /// The member associated with the borrow record.
    /// </summary>
    public virtual Member Member { get; set; }

    public BorrowRecord()
    {

    }

    public BorrowRecord(int id, int bookId, int memberId,
        DateTime borrowDate, DateTime returnDate)
    {
        Id = id;
        BookId = bookId;
        MemberId = memberId;
        BorrowDate = borrowDate;
        ReturnDate = returnDate;
    }
}
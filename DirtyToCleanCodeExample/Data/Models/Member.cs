using Core.Data;

namespace Data.Models;

public class Member : BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public int BorrowCount { get; set; }
    public DateTime MembershipDate { get; set; }

    /// <summary>
    /// The collection of borrow records associated with the member.
    /// </summary>
    public virtual ICollection<BorrowRecord> BorrowRecords { get; set; }

    public Member()
    {
        BorrowRecords = new HashSet<BorrowRecord>();
    }

    public Member(int id, string name, string email,
        int borrowCount, DateTime membershipDate)
    {
        Id = id;
        Name = name;
        Email = email;
        BorrowCount = borrowCount;
        MembershipDate = membershipDate;
    }
}
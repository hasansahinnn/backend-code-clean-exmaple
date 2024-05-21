using System;
namespace Data.Example2
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime PublishedDate { get; set; }
        public ICollection<BorrowRecord> BorrowRecords { get; set; }
    }

    public class Member
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int BorrowCount { get; set; }
        public DateTime MembershipDate { get; set; }
        public ICollection<BorrowRecord> BorrowRecords { get; set; }
    }

    public class BorrowRecord
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int MemberId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public Book Book { get; set; }
        public Member Member { get; set; }
    }
}


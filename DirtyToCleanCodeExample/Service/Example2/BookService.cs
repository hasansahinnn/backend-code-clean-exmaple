using System.Text;
using Core.Services;
using Data;
using Data.Example2;
using Microsoft.EntityFrameworkCore;

namespace Service.Example2
{
    public class BookService
    {
        private readonly DataContext _dbContext;
        private readonly EmailService _emailService;

        public BookService(DataContext dbContext, EmailService emailService)
        {
            _dbContext = dbContext;
            _emailService = emailService;
        }
     

        public async Task<string> BorrowBooksAsync(int memberId, List<int> bookIds)
        {
            var member = await _dbContext.Members.FirstOrDefaultAsync(m => m.Id == memberId);
            if (member == null)
            {
                return "Member not found.";
            }

            var books = await _dbContext.Books
                .Where(b => bookIds.Contains(b.Id))
                .ToListAsync();

            if (books.Count != bookIds.Count)
            {
                var missingBookIds = bookIds.Except(books.Select(b => b.Id));
                return $"Book(s) with ID(s) {string.Join(", ", missingBookIds)} not found.";
                // Kayıp kitapları döndürmek yerine, hata mesajında belirtmek daha iyi olabilirdi
            }

            foreach (var book in books)
            {
                var borrowRecord = new BorrowRecord
                {
                    BookId = book.Id,
                    MemberId = member.Id,
                    BorrowDate = DateTime.Now
                };
                _dbContext.BorrowRecords.Add(borrowRecord);
            }

            member.BorrowCount++;
            _dbContext.Members.Update(member);

            await _dbContext.SaveChangesAsync();

            try
            {
                await _emailService.SendEmailAsync(member.Email, "Books Borrowed", $"You have borrowed the following books. Use this Permission Code for Confirmation --> 1421:\n{string.Join("\n", books.Select(b => b.Title))}");
            }
            catch (Exception ex)
            {
                return $"Books borrowed but failed to send email: {ex.Message}";
            }

            return "Books borrowed and email sent successfully.";
        }

        private async Task SendEmailsForRecentBorrowersAsync(int daysAgo)
        {
            await foreach (var report in GetRecentBorrowersAsync(daysAgo))
            {
                await _emailService.SendEmailAsync("admin@example.com", "Recent Book Borrowers Report", report);
            }
        }

        private async IAsyncEnumerable<string> GetRecentBorrowersAsync(int daysAgo)
        {
            var startDate = DateTime.Now.AddDays(-daysAgo);

            for (int i = 0; i <= daysAgo; i++)
            {
                var targetDate = startDate.AddDays(i);
                var borrowRecords = await _dbContext.BorrowRecords
                    .Where(br => br.BorrowDate.Date == targetDate.Date)
                    .GroupBy(br => br.MemberId)
                    .ToListAsync();

                var reportBuilder = new StringBuilder();
                foreach (var group in borrowRecords)
                {
                    var user = await _dbContext.Users.FindAsync(group.Key);
                    if (user == null)  // User null ise devam etmek yerine hata mesajı döndürülebilir
                        continue;
                    var bookTitles = await _dbContext.Books
                        .Where(b => group.Any(br => br.BookId == b.Id))
                        .Select(b => b.Title)
                        .ToListAsync();

                    reportBuilder.AppendLine($"{user.Username} - ({string.Join(", ", bookTitles)})");
                }
                yield return reportBuilder.ToString();
            }
        }

    }
}

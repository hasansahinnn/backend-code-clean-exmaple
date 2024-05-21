using System;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
using Data;
using Microsoft.AspNetCore.Http;
using Data.Example2;
using System.Text;

namespace Service.Example2
{
	public class BookService
	{
        private readonly DataContext _dbContext;
        public BookService(IHttpContextAccessor httpContextAccessor, DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> BorrowBooksAsync(int memberId, List<int> bookIds)
        {
            var member = await _dbContext.Members.FirstOrDefaultAsync(m => m.Id == memberId);
            if (member == null)
            {
                return "Member not found.";
            }

            var books = new List<Book>();
            foreach (var bookId in bookIds)
            {
                var book = await _dbContext.Books.FirstOrDefaultAsync(b => b.Id == bookId);
                if (book == null)
                {
                    return $"Book with ID {bookId} not found.";
                }
                books.Add(book);
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
                // E-posta gönderme işlemi, bu mail kesinlikle atilmasi gerekiyor
                var smtpClient = new SmtpClient("smtp.example.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("username", "password"),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("library@example.com"),
                    Subject = "Books Borrowed",
                    Body = "You have borrowed the following books. Use this Permission Code for Confirmation --> 1421:\n" + string.Join("\n", books.Select(b => b.Title)),
                    IsBodyHtml = false,
                };
                mailMessage.To.Add(member.Email);

                await smtpClient.SendMailAsync(mailMessage);

            }
            catch (Exception ex)
            {
                return $"Books borrowed but failed to send email: {ex.Message}";
            }

            return "Books borrowed and email sent successfully.";
        }

        public async Task SendEmailsForRecentBorrowersAsync(int daysAgo)
        {
            await foreach (var report in GetRecentBorrowersAsync(daysAgo))
            {
                using (var smtpClient = new SmtpClient("smtp.example.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("username", "password"),
                    EnableSsl = true,
                })
                {
                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress("library@example.com"),
                        Subject = "Recent Book Borrowers Report",
                        Body = report,
                        IsBodyHtml = false,
                    };
                    // Burada gerçek e-posta adresini belirtmelisiniz
                    mailMessage.To.Add("admin@example.com");

                    await smtpClient.SendMailAsync(mailMessage);
                }
            }
        }

        public async IAsyncEnumerable<string> GetRecentBorrowersAsync(int daysAgo)
        {
            var startDate = DateTime.Now.AddDays(-daysAgo);

            var reportBuilder = new StringBuilder();

            for (int i = 0; i <= daysAgo; i++)
            {
                var targetDate = startDate.AddDays(i);
                var borrowRecords = await _dbContext.BorrowRecords
                    .Where(br => br.BorrowDate.Date == targetDate.Date)
                    .GroupBy(br => br.MemberId)
                    .ToListAsync();

                foreach (var group in borrowRecords)
                {
                    var user = await _dbContext.Users.FindAsync(group.Key);
                    var bookTitles = await _dbContext.Books
                        .Where(b => group.Select(br => br.BookId).Contains(b.Id))
                        .Select(b => b.Title)
                        .ToListAsync();

                    reportBuilder.AppendLine($"{user.Username} - ({string.Join(", ", bookTitles)})");
                }
            }

            yield return reportBuilder.ToString();
        }
    }
}


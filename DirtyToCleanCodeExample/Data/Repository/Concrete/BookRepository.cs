using Core.Example1;
using Core.Example2;
using Data.Repository.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Core.ReturnModel;
using AutoMapper.Execution;

namespace Data.Repository.Concrete
{
    public class BookRepository : Repository<Book, DataContext>
    {
        public BookRepository(DataContext context) : base(context)
        {
        }


        public async Task SendEmailAsync(string smtpServer, int port, string username, string password, string fromEmail, string toEmail, string mailText, MailMessage mailMessage)
        {
            using (var smtpClient = new SmtpClient(smtpServer)
            {
                Port = port,
                Credentials = new NetworkCredential(username, password),
                EnableSsl = true,
            })
            {
                mailMessage.To.Add(toEmail);

                mailMessage.Body = mailText;

                await smtpClient.SendMailAsync(mailMessage);
            }
        }

        public async Task<IReturn> BorrowBooksAsync(int memberId, List<int> bookIds)
        {
            var member = await context.Members.FirstOrDefaultAsync(m => m.Id == memberId);
            if (member == null)
            {
                return new ErrorReturn("üye yok");
            }

            var books = await context.Books.Where(p => bookIds.Contains(p.Id)).ToListAsync();
            if(books != null)
            {
                List<BorrowRecord> borrowRecords = new List<BorrowRecord>();

                books.ForEach(p => borrowRecords.Add(new BorrowRecord() { BookId = p.Id, MemberId=member.Id, BorrowDate=DateTime.Now }));

                await context.BorrowRecords.AddRangeAsync(borrowRecords);

                await SaveAsync();
            }
            else
            {
                return new ErrorReturn();
            }

            try
            {
                // E-posta gönderme işlemi, bu mail kesinlikle atilmasi gerekiyor

                // mail gönderimi bilmiyorum kısaltma vs aklıma bir şey gelmedi chatgpt ile yaptım

                var bodyBuilder = new StringBuilder();
                bodyBuilder.AppendLine("You have borrowed the following books. Use this Permission Code for Confirmation --> 1421:");
                foreach (var book in books)
                {
                    bodyBuilder.AppendLine(book.Title);
                }
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("library@example.com"),
                    Subject = "Books Borrowed",
                    Body = "You have borrowed the following books. Use this Permission Code for Confirmation --> 1421:\n" + string.Join("\n", books.Select(b => b.Title)),
                    IsBodyHtml = false,
                };
                await SendEmailAsync("smtp.example.com", 587, "username", "password", "library@example.com", member.Email, bodyBuilder.ToString(), mailMessage);
            }
            catch (Exception ex)
            {
                return new ErrorReturn(ex);
            }

            return new SuccessReturn();
        }





        public async Task<IReturn> SendEmailsForRecentBorrowersAsync(int daysAgo)
        {
            await foreach (var report in GetRecentBorrowersAsync(daysAgo))
            {
                {
                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress("library@example.com"),
                        Subject = "Recent Book Borrowers Report",
                        Body = report,
                        IsBodyHtml = false,
                    };
                    await SendEmailAsync("smtp.example.com", 587, "username", "password", "library@example.com", "admin@example.com", "", mailMessage);
                }
            }
            return new SuccessReturn();
        }

        public async IAsyncEnumerable<string> GetRecentBorrowersAsync(int daysAgo)
        {// chatgpt ye verdim direkt kafam kalmadı fazla
            var startDate = DateTime.Now.AddDays(-daysAgo);
            var endDate = DateTime.Now;

            var borrowRecords = await context.BorrowRecords
                .Where(br => br.BorrowDate >= startDate && br.BorrowDate <= endDate)
                .GroupBy(br => br.MemberId)
                .ToListAsync();

            foreach (var group in borrowRecords)
            {
                var user = await context.Users.FindAsync(group.Key);
                var bookIds = group.Select(br => br.BookId).Distinct();
                var bookTitles = await context.Books
                    .Where(b => bookIds.Contains(b.Id))
                    .Select(b => b.Title)
                    .ToListAsync();

                var reportBuilder = new StringBuilder();
                reportBuilder.AppendLine($"{user.Username} - ({string.Join(", ", bookTitles)})");
                yield return reportBuilder.ToString();
            }
        }





    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Data.Example2;
using System.Net.Http;

namespace Service.Example2
{
    public class EmailService
    {
        public async Task SendEmailForBorrowsBookAsync(string email,List<Book> books)
        {
            // EmailService içinde bir fonksiyon yazıp çağırmak lazım.
            try
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
                        Subject = "Books Borrowed",
                        Body = "You have borrowed the following books. Use this Permission Code for Confirmation --> 1421:\n" + string.Join("\n", books.Select(b => b.Title)),
                        IsBodyHtml = false,
                    };
                    mailMessage.To.Add(email);
                    await smtpClient.SendMailAsync(mailMessage);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while sending email.", ex);
            }

        }
        public async Task SendEmailForBorrowsBookReportAsync(string email,string body)
        {
            try
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
                        Body = body,
                        IsBodyHtml = false,
                    };
                    mailMessage.To.Add(email);

                    await smtpClient.SendMailAsync(mailMessage);
                }
            }
            catch(Exception ex)
            {
                throw new ApplicationException("An error occurred while sending email.", ex);
            }
        }
    }
}

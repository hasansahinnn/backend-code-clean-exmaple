using System.Net;
using System.Net.Mail;

namespace Core.Services;

public class EmailService
{
    // Email servisi kullanılarak belirtilen alıcıya e-posta gönderir.
    // Core katmanında olmasının sebebi businessdan bağımsız olmasıdır. Diğer projelerde de kullanılabilir.
    public async Task SendEmailAsync(string recipient, string subject, string body)
    {
        using var smtpClient = new SmtpClient("smtp.example.com");
        smtpClient.Port = 587;
        smtpClient.Credentials = new NetworkCredential("username", "password");
        smtpClient.EnableSsl = true;

        using var mailMessage = new MailMessage();
        mailMessage.From = new MailAddress("library@example.com");
        mailMessage.To.Add(recipient);
        mailMessage.Subject = subject;
        mailMessage.Body = body;
        mailMessage.IsBodyHtml = false;

        await smtpClient.SendMailAsync(mailMessage);
    }
}

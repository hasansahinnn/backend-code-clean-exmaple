using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Core.Mailing;

/// <summary>
/// Mail service class
/// <para>Contains the mail service methods.</para>
/// <para>Every mail service class should inherit from this class.</para>
/// </summary>
public class MailKitMailService : IMailService
{
    private readonly MailSettings? _mailSettings;

    // Mail ayarlarını içeren yapıyı enjekte eden constructor
    public MailKitMailService(IConfiguration configuration)
    {

        _mailSettings = configuration.GetSection("MailSettings").Get<MailSettings>();
    }

    /// <summary>
    /// Send mail method
    /// <para>Sends the mail.</para>
    /// </summary>
    public void SendMail(Mail mail)
    {
        if (mail.ToList == null || mail.ToList.Count < 1)
            return;

        EmailPrepare(mail, email: out MimeMessage email, smtp: out SmtpClient smtp);

        smtp.Send(email);
        smtp.Disconnect(true);
        email.Dispose();
        smtp.Dispose();
    }

    /// <summary>
    /// Send email async method
    /// <para>Sends the email asynchronously.</para>
    /// <para>Contains the mail parameter.</para>
    /// </summary>
    public async Task SendEmailAsync(Mail mail)
    {
        if (mail.ToList == null || mail.ToList.Count < 1)
            return;

        EmailPrepare(mail, email: out MimeMessage email, smtp: out SmtpClient smtp);
        
        await smtp.SendAsync(email);
        
        await smtp.DisconnectAsync(true);
        email.Dispose();
        smtp.Dispose();
    }

    /// <summary>
    /// Prepares the email
    /// <para>Prepares the email to be sent.</para>
    /// </summary>
    /// <param name="mail"></param>
    /// <param name="email"></param>
    /// <param name="smtp"></param>
    private void EmailPrepare(Mail mail, out MimeMessage email, out SmtpClient smtp)
    {
        // Create a mail object
        email = new MimeMessage();
        email.From.Add(new MailboxAddress(_mailSettings.SenderFullName, _mailSettings.SenderEmail));
        email.To.AddRange(mail.ToList);

        // CC and BCC lists are optional so we need to check if they are null or empty
        if (mail.CcList != null && mail.CcList.Any())
            email.Cc.AddRange(mail.CcList);
        if (mail.BccList != null && mail.BccList.Any())
            email.Bcc.AddRange(mail.BccList);

        email.Subject = mail.Subject;

        // Add the List-Unsubscribe header if the UnsubscribeLink is not null
        if (mail.UnsubscribeLink != null)
            email.Headers.Add(field: "List-Unsubscribe", value: $"<{mail.UnsubscribeLink}>");

        // Create the body of the email
        BodyBuilder bodyBuilder = new() { TextBody = mail.TextBody, HtmlBody = mail.HtmlBody };
        email.Body = bodyBuilder.ToMessageBody();
        email.Prepare(EncodingConstraint.SevenBit);

        smtp = new SmtpClient();
        smtp.Connect(_mailSettings.Server, _mailSettings.Port,  SecureSocketOptions.StartTls);
        // If the server requires authentication, authenticate
        if (_mailSettings.AuthenticationRequired)
            smtp.Authenticate(_mailSettings.UserName, _mailSettings.Password);
    }
}
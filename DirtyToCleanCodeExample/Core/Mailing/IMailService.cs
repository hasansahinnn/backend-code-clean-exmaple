namespace Core.Mailing;

/// <summary>
/// Interface for mail service
/// <para>Every mail service class should implement this interface.</para>
/// <para>Contains the SendMail and SendEmailAsync methods.</para>
/// </summary>
/// <seealso cref="Mail"/>
/// <seealso cref="IMailService"/>
/// <seealso cref="MailService"/>
public interface IMailService
{
    /// <summary>
    /// Send mail method
    /// <para>Sends the mail.</para>
    /// </summary>
    /// <param name="mail"></param>
    void SendMail(Mail mail);
    /// <summary>
    /// Send email async method
    /// <para>Sends the email asynchronously.</para>
    /// <para>Contains the mail parameter.</para>
    /// </summary>
    /// <param name="mail"></param>
    Task SendEmailAsync(Mail mail);
}

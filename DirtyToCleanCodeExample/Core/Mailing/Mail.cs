using MimeKit;

namespace Core.Mailing;

/// <summary>
/// Mail class
/// <para>Contains the properties of the mail.</para>
/// <para>Every mail should be an instance of this class.</para>
/// </summary>
public class Mail
{
    /// <summary>
    /// Subject of the mail
    /// <para>Contains the subject of the mail.</para>
    /// </summary>
    public string Subject { get; set; }
    /// <summary>
    /// Text body of the mail
    /// <para>Contains the text body of the mail.</para>
    /// </summary>
    public string TextBody { get; set; }
    /// <summary>
    /// Html body of the mail
    /// <para>Contains the html body of the mail.</para>
    /// </summary>
    public string HtmlBody { get; set; }
    /// <summary>
    /// List of mail addresses
    /// <para>Contains the list of mail addresses.</para>
    /// </summary>
    public List<MailboxAddress> ToList { get; set; }
    /// <summary>
    /// List of mail addresses
    /// <para>Contains the list of mail addresses.</para>
    /// </summary>
    public List<MailboxAddress>? CcList { get; set; }
    /// <summary>
    /// List of mail addresses
    /// <para>Contains the list of mail addresses.</para>
    /// </summary>
    public List<MailboxAddress>? BccList { get; set; }
    /// <summary>
    /// Reply to mail address
    /// <para>Contains the reply to mail address.</para>
    /// </summary>
    public string? UnsubscribeLink { get; set; }

    public Mail()
    {
        Subject = string.Empty;
        TextBody = string.Empty;
        HtmlBody = string.Empty;
        ToList = new List<MailboxAddress>();
    }

    public Mail(string subject, string textBody, string htmlBody, List<MailboxAddress> toList)
    {
        Subject = subject;
        TextBody = textBody;
        HtmlBody = htmlBody;
        ToList = toList;
    }
}

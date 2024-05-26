namespace Core.Mailing
{
    /// <summary>
    /// Represents the settings required for email configuration.
    /// </summary>
    public class MailSettings
    {
        /// <summary>
        /// Gets or sets the SMTP server address.
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// Gets or sets the port number for the SMTP server.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the full name of the email sender.
        /// </summary>
        public string SenderFullName { get; set; }

        /// <summary>
        /// Gets or sets the email address of the sender.
        /// </summary>
        public string SenderEmail { get; set; }

        /// <summary>
        /// Gets or sets the username for authentication with the SMTP server.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password for authentication with the SMTP server.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets whether authentication is required when connecting to the SMTP server.
        /// </summary>
        public bool AuthenticationRequired { get; set; }

        /// <summary>
        /// Gets or sets the DKIM private key for email authentication.
        /// </summary>
        public string? DkimPrivateKey { get; set; }

        /// <summary>
        /// Gets or sets the DKIM selector for email authentication.
        /// </summary>
        public string? DkimSelector { get; set; }

        /// <summary>
        /// Gets or sets the domain name for email authentication.
        /// </summary>
        public string? DomainName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MailSettings"/> class.
        /// </summary>
        public MailSettings()
        {
            // Default constructor
            Server = string.Empty;
            Port = 0;
            SenderFullName = string.Empty;
            SenderEmail = string.Empty;
            UserName = string.Empty;
            Password = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MailSettings"/> class with specified parameters.
        /// </summary>
        /// <param name="server">SMTP server address.</param>
        /// <param name="port">Port number for the SMTP server.</param>
        /// <param name="senderFullName">Full name of the email sender.</param>
        /// <param name="senderEmail">Email address of the sender.</param>
        /// <param name="userName">Username for authentication with the SMTP server.</param>
        /// <param name="password">Password for authentication with the SMTP server.</param>
        /// <param name="authenticationRequired">Specifies whether authentication is required when connecting to the SMTP server.</param>
        public MailSettings(
            string server,
            int port,
            string senderFullName,
            string senderEmail,
            string userName,
            string password,
            bool authenticationRequired
        )
        {
            Server = server;
            Port = port;
            SenderFullName = senderFullName;
            SenderEmail = senderEmail;
            UserName = userName;
            Password = password;
            AuthenticationRequired = authenticationRequired;
        }
    }
}

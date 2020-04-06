using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using Simix.Extensions.Logging.Abstractions;
using System.Collections.Generic;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Simix.Extensions.Logging {
    /// <summary>
    /// Authenticated message sender
    /// </summary>
    public class AuthMessageSender : IEmailSender {
        #region Fields

        private readonly ISmtpConfig _emailSettings;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="emailSettings"></param>
        public AuthMessageSender(IOptions<SmtpConfig> emailSettings) =>
            _emailSettings = emailSettings.Value;

        #endregion

        #region Pubic Methods

        /// <summary>
        /// Sends an authenticated email message
        /// </summary>
        /// <param name="email"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendEmailAsync(string email, string subject, string message) {
            var mail = new MimeMessage();

            mail.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
            mail.To.Add(new MailboxAddress(email));
            SetDestination(mail, _emailSettings.Destination);

            mail.Subject = subject;
            mail.Body = BuildBodyMessage(message);
            mail.Priority = GetEmailPriority();

            using (var client = new SmtpClient()) {
                await ConnectAndSendMessageAsync(mail, client, _emailSettings).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Build the email body message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected virtual MimeEntity BuildBodyMessage(string message) =>
            new TextPart(TextFormat.Html) { Text = message };

        /// <summary>
        /// Get the email priority
        /// </summary>
        /// <returns></returns>
        protected virtual MessagePriority GetEmailPriority() => MessagePriority.Urgent;

        /// <summary>
        /// Process SmtpClient authentication and send email message
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="client"></param>
        /// <param name="smtpConfig"></param>
        /// <returns></returns>
        protected virtual async Task ConnectAndSendMessageAsync(MimeMessage mail, ISmtpClient client, ISmtpConfig smtpConfig) {
            client.ServerCertificateValidationCallback = ServerValidationCallback;
            client.Connect(smtpConfig.Host, smtpConfig.Port, smtpConfig.EnableSsl);
            client.Authenticate(smtpConfig.Username, smtpConfig.Password);

            await client.SendAsync(mail).ConfigureAwait(false);
            client.Disconnect(true);
        }

        /// <summary>
        /// Set all email destinations
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="destination"></param>
        protected virtual void SetDestination(MimeMessage mail, IEnumerable<string> destination) {
            foreach (var cc in destination)
                mail.To.Add(new MailboxAddress(cc));
        }

        /// <summary>
        /// Server ssl certificate validation callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="sslPolicyErrors"></param>
        /// <returns></returns>
        protected virtual bool ServerValidationCallback(
            object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
            return true;
        }

        #endregion
    }
}

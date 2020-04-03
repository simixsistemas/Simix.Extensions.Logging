using MailKit.Net.Smtp;
using MimeKit;
using Simix.Extensions.Logging.Abstractions;
using Simix.SuperMidia.Services.Helpers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Simix.Extensions.Logging {
    public class AuthMessageSender : IEmailSender {
        #region Fields

        private readonly SmtpDetails _emailSettings;

        #endregion

        #region Constructors

        public AuthMessageSender(SmtpDetails emailSettings) =>
            _emailSettings = emailSettings;

        #endregion

        #region Pubic Methods

        public async Task SendEmailAsync(string email, string subject, string message) =>
            await Execute(email, subject, message).ConfigureAwait(false);

        public async Task Execute(string email, string subject, string message) {
            var mail = new MimeMessage();

            mail.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
            mail.To.Add(new MailboxAddress(email));

            foreach (var cc in _emailSettings.Destination)
                mail.To.Add(new MailboxAddress(cc));

            mail.Subject = subject;
            mail.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message };
            mail.Priority = MessagePriority.Urgent;

            using (var client = new SmtpClient()) {
                client.ServerCertificateValidationCallback = ServerValidationCallback;
                client.Connect(_emailSettings.Host, _emailSettings.Port, _emailSettings.EnableSsl);
                client.Authenticate(_emailSettings.UserName, _emailSettings.Password);

                await client.SendAsync(mail).ConfigureAwait(false);
                client.Disconnect(true);
            }
        }

        protected virtual bool ServerValidationCallback(
            object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
            return true;
        }

        #endregion
    }
}

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Simix.Extensions.Logging.Abstractions;
using Simix.Extensions.Logging.Extensions;
using Simix.Extensions.Logging.Helpers;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Simix.Extensions.Logging {
    /// <summary>
    /// Email logger class
    /// </summary>
    public class EmailLogger : IEmailLogger {
        #region Fields

        private readonly SmtpConfig _smtpDetails;
        private readonly LoggerDetails _loggerDetails;

        private readonly IEmailSender _emailSender;

        #endregion

        #region Constructors

        /// <summary>
        /// Email logger constructor
        /// </summary>
        /// <param name="smtpDetails">Smtp details</param>
        /// <param name="loggerDetails">Logger details</param>
        /// <param name="emailSender">IEmailSender implementation</param>
        public EmailLogger(
           IOptions<SmtpConfig> smtpDetails, IOptions<LoggerDetails> loggerDetails, IEmailSender emailSender) {
            _emailSender = emailSender;
            _smtpDetails = smtpDetails.Value;
            _loggerDetails = loggerDetails.Value;
        }

        #endregion

        #region IEmailLogger

        /// <summary>
        /// Log some state
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="logLevel"></param>
        /// <param name="eventId"></param>
        /// <param name="state"></param>
        /// <param name="exception"></param>
        /// <param name="formatter"></param>
        public virtual void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
            Exception exception, Func<TState, Exception, string> formatter) {
            var message = formatter(state, exception);
            SendEmail(message);
        }

        /// <summary>
        /// Check if EmailLogger is enabled
        /// </summary>
        /// <param name="logLevel"></param>
        /// <returns></returns>
        public virtual bool IsEnabled(LogLevel logLevel) {
            if (_smtpDetails == null)
                return false;

            if (string.IsNullOrWhiteSpace(_smtpDetails.Domain))
                return false;


            if (string.IsNullOrWhiteSpace(_smtpDetails.Host))
                return false;

            return true;
        }

        /// <summary>
        /// BeginScope
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="state"></param>
        /// <returns></returns>
        public virtual IDisposable BeginScope<TState>(TState state) => new NoDispose();

        #endregion

        #region Protected Methods

        /// <summary>
        /// Sends the email message
        /// </summary>
        /// <param name="message"></param>
        protected virtual void SendEmail(string message) {
            var stringBuilder = new StringBuilder();
            BuildEmailBody(stringBuilder, HtmlHelper.CreateHeaderTitle(_loggerDetails.LogHeader ?? "New Log"), message);
            SendEmailAsync(stringBuilder)
                .SafeFireAndForget(onException: ex => Debug.WriteLine(ex));
        }

        /// <summary>
        /// Send the email message
        /// </summary>
        /// <param name="stringBuilder"></param>
        /// <returns></returns>
        protected virtual async Task SendEmailAsync(StringBuilder stringBuilder) {
            await _emailSender.SendEmailAsync(
                _smtpDetails.SenderEmail, _loggerDetails.ServiceName ?? "Log Service", stringBuilder.ToString());
        }

        /// <summary>
        /// Build the email body message
        /// </summary>
        /// <param name="stringBuilder"></param>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        protected virtual StringBuilder BuildEmailBody(StringBuilder stringBuilder, string title, string message) {
            stringBuilder
                .Append(title)
                .AppendLine(Constants.HtmlTag.LINE_BREAK)
                .Append(message)
                .AppendLine(Constants.HtmlTag.LINE_BREAK);

            return stringBuilder;
        }

        #endregion
    }

    /// <summary>
    /// No dipose class
    /// </summary>
    [Serializable]
    public class NoDispose : IDisposable {
        /// <summary>
        /// Dispose without implementation
        /// </summary>
        public void Dispose() { }
    }
}

using Microsoft.Extensions.Logging;
using Simix.Extensions.Logging.Abstractions;
using Simix.Extensions.Logging.Helpers;
using Simix.SuperMidia.Core.Extensions;
using Simix.SuperMidia.Services.Abstractions;
using Simix.SuperMidia.Services.Helpers;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Simix.Extensions.Logging {
    public class EmailLogger : IEmailLogger {
        #region Fields

        private readonly SmtpDetails _smtpDetails;
        private readonly LoggerDetails _loggerDetails;

        private readonly IEmailSender _emailSender;

        #endregion

        #region Constructors

        public EmailLogger(
           SmtpDetails smtpDetails, LoggerDetails loggerDetails, IEmailSender emailSender) {
            _emailSender = emailSender;
            _smtpDetails = smtpDetails;
            _loggerDetails = loggerDetails;
        }

        #endregion

        #region IEmailLogger

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
            Exception exception, Func<TState, Exception, string> formatter) {
            var message = formatter(state, exception);
            SendEmailInfo(message);
        }

        public virtual bool IsEnabled(LogLevel logLevel) {
            if (_smtpDetails == null)
                return false;

            if (string.IsNullOrWhiteSpace(_smtpDetails.Domain))
                return false;


            if (string.IsNullOrWhiteSpace(_smtpDetails.Host))
                return false;

            return true;
        }

        public IDisposable BeginScope<TState>(TState state) => new NoDispose();

        #endregion

        #region Protected Methods

        protected virtual void SendEmailInfo(string message) {
            var stringBuilder = new StringBuilder();
            BuildEmailBody(stringBuilder, HtmlHelper.CreateHeaderTitle(_loggerDetails.LogHeader ?? "New Log"), message);
            SendEmailAsync(stringBuilder)
                .SafeFireAndForget(onException: ex => Debug.WriteLine(ex));
        }

        protected virtual async Task SendEmailAsync(StringBuilder stringBuilder) {
            await _emailSender.SendEmailAsync(
                _smtpDetails.SenderEmail, _loggerDetails.ServiceName ?? "Log Service", stringBuilder.ToString());
        }

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

    [Serializable]
    public class NoDispose : IDisposable {
        public void Dispose() { }
    }
}

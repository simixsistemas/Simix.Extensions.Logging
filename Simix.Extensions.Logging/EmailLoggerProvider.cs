using Microsoft.Extensions.Logging;
using Simix.Extensions.Logging.Abstractions;

namespace Simix.Extensions.Logging {
    /// <summary>
    /// EmailLoggerProvider
    /// </summary>
    public sealed class EmailLoggerProvider : IEmailLoggerProvider {
        private readonly IEmailLogger _emailLogger;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="emailLogger"></param>
        public EmailLoggerProvider(IEmailLogger emailLogger) => _emailLogger = emailLogger;

        /// <summary>
        /// ILogger builder
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public ILogger CreateLogger(string categoryName) => _emailLogger;

        /// <summary>
        /// IDisposable
        /// </summary>
        public void Dispose() { }
    }
}

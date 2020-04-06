using System.Collections.Generic;

namespace Simix.Extensions.Logging.Abstractions {
    /// <summary>
    /// SmtpConfig abstraction
    /// </summary>
    public interface ISmtpConfig {
        /// <summary>
        /// Smtp host
        /// </summary>
        public string Host { get; }

        /// <summary>
        /// Smtp username
        /// </summary>
        public string Username { get; }

        /// <summary>
        /// Smtp password
        /// </summary>
        public string Password { get; }

        /// <summary>
        /// Smtp port
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// Defines to use ssl
        /// </summary>
        public bool EnableSsl { get; }

        /// <summary>
        /// Smtp domain
        /// </summary>
        public string Domain { get; }

        /// <summary>
        /// Smtp sender email
        /// </summary>
        public string SenderEmail { get; }

        /// <summary>
        /// Smtp sender name
        /// </summary>
        public string SenderName { get; }

        /// <summary>
        /// Smtp destination
        /// </summary>
        public IEnumerable<string> Destination { get; }
    }
}

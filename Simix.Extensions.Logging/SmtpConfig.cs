using Newtonsoft.Json;
using Simix.Extensions.Logging.Abstractions;
using System.Collections.Generic;

namespace Simix.Extensions.Logging {
    /// <summary>
    /// Smtp config
    /// </summary>
    public class SmtpConfig : ISmtpConfig {
        /// <summary>
        /// Smtp Host
        /// </summary>
        [JsonProperty("host")]
        public string Host { get; set; }

        /// <summary>
        /// Smtp Username
        /// </summary>
        [JsonProperty("username")]
        public string Username { get; set; }

        /// <summary>
        /// Smtp Password
        /// </summary>
        [JsonProperty("password")]
        public string Password { get; set; }

        /// <summary>
        /// Smtp port
        /// </summary>
        [JsonProperty("port")]
        public int Port { get; set; }

        /// <summary>
        /// Defines to use ssl
        /// </summary>
        [JsonProperty("enablessl")]
        public bool EnableSsl { get; set; }

        /// <summary>
        /// Smtp domain
        /// </summary>
        [JsonProperty("domain")]
        public string Domain { get; set; }

        /// <summary>
        /// Smtp sender email
        /// </summary>
        [JsonProperty("senderEmail")]
        public string SenderEmail { get; set; }

        /// <summary>
        /// Smtp sender name
        /// </summary>
        [JsonProperty("senderName")]
        public string SenderName { get; set; }

        /// <summary>
        /// Smtp Destination
        /// </summary>
        [JsonProperty("destination")]
        public IEnumerable<string> Destination { get; set; }
    }
}

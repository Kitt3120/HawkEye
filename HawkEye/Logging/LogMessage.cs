using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HawkEye.Logging
{
    /// <summary>
    /// Represents a single message that is contained in a LoggingSection.
    /// </summary>
    public class LogMessage
    {
        /// <summary>
        /// The LoggingSection this LogMessage belongs to.
        /// </summary>
        public LoggingSection LoggingSection { get; private set; }

        /// <summary>
        /// The LogLevel defined for the LogMessage.
        /// </summary>
        public LogLevel LogLevel { get; private set; }

        /// <summary>
        /// The message content.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// The timestamp of the LogMessage's creation.
        /// </summary>
        public DateTime Timestamp { get; private set; }

        /// <summary>
        /// Constructs a LogMessage.
        /// Not intended for manual usage. Instead, use LoggingSection.CreateLogMessage();
        /// </summary>
        /// <param name="loggingSection">The LoggingSection the LogMessage belongs to.</param>
        /// <param name="logLevel">The LogLevel to be used for the LogMessage.</param>
        /// <param name="message">The message content to be used for the LogMessage.</param>
        internal LogMessage(LoggingSection loggingSection, LogLevel logLevel, string message)
        {
            if (loggingSection == null)
                throw new ArgumentNullException("loggingSection mustn't be null.");

            LoggingSection = loggingSection;
            LogLevel = logLevel;
            Message = message;
            Timestamp = DateTime.Now;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HawkEye.Logging
{
    /// <summary>
    /// Represents a single message that is contained in a LoggingSection
    /// </summary>
    public class LogMessage
    {
        public LoggingSection LoggingSection { get; private set; }
        public LogLevel LogLevel { get; private set; }
        public string Message { get; private set; }
        public DateTime Timestamp { get; private set; }

        public LogMessage(LoggingSection loggingSection, LogLevel logLevel, string message)
        {
            if (loggingSection == null)
                throw new ArgumentNullException("loggingSection mustn't be null.");

            LoggingSection = loggingSection;
            LogLevel = logLevel;
            Message = message;
            Timestamp = DateTime.Now;
        }

        public override string ToString()
        {
            return $"{Timestamp.ToString("dd.MM.yyyy hh:mm:ss")} - [{LogLevel}] - [{LoggingSection.FullPath}]: {Message}";
        }

        public string ToShortString()
        {
            return $"[{LogLevel}] - [{LoggingSection.FullPath}]: {Message}";
        }
    }
}
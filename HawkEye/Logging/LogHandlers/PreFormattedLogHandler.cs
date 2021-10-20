using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HawkEye.Logging.LogHandlers
{
    /// <summary>
    /// An abstract LogHandler that converts LogMessages into a string according to an editable pattern before handing it over to the implementation.
    /// Used to reduce redundant LogMessage to string conversions across different LogHandler implementations.
    /// </summary>
    public abstract class PreFormattedLogHandler : LogHandler
    {
        /// <summary>
        /// The pattern that is used for converting LogMessages into a string.
        /// </summary>
        public string Format { get; private set; } = "%datetime% - [%loglevel%] - [%loggingsection%]: %message%";

        /// <summary>
        /// The pattern that is used for converting a DateTime object into a string.
        /// </summary>
        public string DateTimeFormat { get; set; }

        /// <summary>
        /// Constructs a PreFormattedLogHandler
        /// </summary>
        /// <param name="format">Pattern according to which a LogMessage is converted into a string. If not provided, a default pattern will be used.</param>
        /// <param name="enabledLogLevels">Enabled LogLevels. If not provided, all LogLevels are enabled by default.</param>
        public PreFormattedLogHandler(string format = null, LogLevel[] enabledLogLevels = null) : base(enabledLogLevels)
        {
            if (format != null)
                Format = format;
        }

        /// <summary>
        /// Converts a LogMessage according to the given pattern and hands over the result to the LogHandler implementation.
        /// </summary>
        /// <param name="logMessage">The LogMessage to be handled</param>
        public override void HandleLogMessage(LogMessage logMessage) => Output(
            Format
            .Replace("%loggingsection%", logMessage.LoggingSection.FullPath)
            .Replace("%loglevel%", logMessage.LogLevel.ToString())
            .Replace("%message%", logMessage.Message)
            .Replace("%datetime%", logMessage.Timestamp.ToString(DateTimeFormat))
            );

        public abstract void Output(string message);
    }
}
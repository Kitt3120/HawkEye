using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HawkEye.Logging.LogHandlers
{
    public abstract class PreFormattedLogHandler : LogHandler
    {
        private static readonly string defaultFormat = "%datetime% - [%loglevel%] - [%loggingsection%]: %message%";
        public string Format { get; private set; }
        public string DateTimeFormat { get; set; }

        public PreFormattedLogHandler(string format = null, LogLevel[] enabledLogLevels = null) : base(enabledLogLevels)
        {
            if (format == null)
                Format = defaultFormat;
            else
                Format = format;
        }

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
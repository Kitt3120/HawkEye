using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HawkEye.Logging.LogHandlers
{
    public abstract class LogHandler
    {
        private List<LogLevel> enabledLogLevels;

        public LogHandler(LogLevel[] enabledLogLevels = null)
        {
            if (enabledLogLevels == null)
                this.enabledLogLevels = new List<LogLevel>((LogLevel[])Enum.GetValues(typeof(LogLevel)));
            else
                this.enabledLogLevels = new List<LogLevel>(enabledLogLevels);
        }

        public void OnLog(object source, LogEventArgs logEventArgs)
        {
            if (IsEnabled(logEventArgs.LogMessage.LogLevel))
                HandleLogMessage(logEventArgs.LogMessage);
        }

        public abstract void HandleLogMessage(LogMessage logMessage);

        public void SetEnabled(LogLevel logLevel, bool enabled)
        {
            if (enabled && !IsEnabled(logLevel))
                enabledLogLevels.Add(logLevel);
            else if (!enabled)
                enabledLogLevels.Remove(logLevel);
        }

        public bool IsEnabled(LogLevel logLevel) => enabledLogLevels.Contains(logLevel);

        public void Enable(LogLevel logLevel) => SetEnabled(logLevel, true);

        public void Disable(LogLevel logLevel) => SetEnabled(logLevel, false);

        public ReadOnlyCollection<LogLevel> GetEnabledLogLevels() => enabledLogLevels.AsReadOnly();
    }
}
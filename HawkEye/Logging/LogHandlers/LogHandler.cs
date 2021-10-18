using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HawkEye.Logging.LogHandlers
{
    public abstract class LogHandler
    {
        private List<LogLevel> enabledLevels;

        public LogHandler(LogLevel[] enabledLogLevels = null)
        {
            if (enabledLogLevels == null)
                enabledLevels = new List<LogLevel>((LogLevel[])Enum.GetValues(typeof(LogLevel)));
            else
                enabledLevels = new List<LogLevel>(enabledLogLevels);
        }

        public abstract void OnLog(object source, LogEventArgs eventArgs);

        public void SetEnabled(LogLevel logLevel, bool enabled)
        {
            if (enabled && !IsEnabled(logLevel))
                enabledLevels.Add(logLevel);
            else if (!enabled)
                enabledLevels.Remove(logLevel);
        }

        public bool IsEnabled(LogLevel logLevel) => enabledLevels.Contains(logLevel);

        public void Enable(LogLevel logLevel) => SetEnabled(logLevel, true);

        public void Disable(LogLevel logLevel) => SetEnabled(logLevel, false);
    }
}
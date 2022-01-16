using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HawkEye.Logging.LogHandlers
{
    /// <summary>
    /// LogHandlers are different implementations on how LogMessages should be treated.
    /// </summary>
    public abstract class LogHandler
    {
        /// <summary>
        /// The LogHandler specific enabled LogLevels.
        /// </summary>
        private List<LogLevel> enabledLogLevels;

        /// <summary>
        /// Constructs a LogHandler
        /// </summary>
        /// <param name="enabledLogLevels">Enabled LogLevels. If not provided, all LogLevels are enabled by default.</param>
        public LogHandler(LogLevel[] enabledLogLevels = null)
        {
            if (enabledLogLevels == null)
                this.enabledLogLevels = new List<LogLevel>((LogLevel[])Enum.GetValues(typeof(LogLevel)));
            else
                this.enabledLogLevels = new List<LogLevel>(enabledLogLevels);
        }

        /// <summary>
        /// Method that can be provided to the Logger to subscribe this LogHandler.
        /// If the conditions are met, the LogMessage contained in the LogEventArgs object will be handled by the LogHandler implementation.
        /// </summary>
        /// <param name="source">Object that triggered the event</param>
        /// <param name="logEventArgs">LogEventArgs object containing the LogMessage</param>
        public void OnLog(object source, LogEventArgs logEventArgs)
        {
            if (IsEnabled(logEventArgs.LogMessage.LogLevel))
                HandleLogMessage(logEventArgs.LogMessage);
        }

        /// <summary>
        /// Actual log implementation. Must get overridden by the LogHandler implementation.
        /// </summary>
        /// <param name="logMessage">The LogMessage to handle</param>
        public abstract void HandleLogMessage(LogMessage logMessage);

        /// <summary>
        /// Enables or disabled LogLevels
        /// </summary>
        /// <param name="logLevel">The specified LogLevel</param>
        /// <param name="enabled">Whether the LogLevel should be enabled or disabled.</param>
        public void SetEnabled(LogLevel logLevel, bool enabled)
        {
            if (enabled && !IsEnabled(logLevel))
                enabledLogLevels.Add(logLevel);
            else if (!enabled)
                enabledLogLevels.Remove(logLevel);
        }

        /// <summary>
        /// Check if a certain LogLevel is enabled for the LogHandler.
        /// </summary>
        /// <param name="logLevel">LogLevel to check</param>
        /// <returns>Whether or not the LogLevel is enabled.</returns>
        public bool IsEnabled(LogLevel logLevel) => enabledLogLevels.Contains(logLevel);

        /// <summary>
        /// Wrapper to enable a LogLevel
        /// </summary>
        /// <param name="logLevel">The LogLevel to be enabled</param>
        public void Enable(LogLevel logLevel) => SetEnabled(logLevel, true);

        /// <summary>
        /// Wrapper to disable a LogLevel
        /// </summary>
        /// <param name="logLevel">The LogLevel to be disabled</param>
        public void Disable(LogLevel logLevel) => SetEnabled(logLevel, false);

        /// <summary>
        /// The enabled LogLevels
        /// </summary>
        /// <returns>Enabled LogLevels as read-only</returns>
        public ReadOnlyCollection<LogLevel> GetEnabledLogLevels() => enabledLogLevels.AsReadOnly();
    }
}
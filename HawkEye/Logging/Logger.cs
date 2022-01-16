using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HawkEye.Logging
{
    /// <summary>
    /// Custom args which are served through an event to subscribed LogHandlers.
    /// </summary>
    public class LogEventArgs : EventArgs
    {
        /// <summary>
        /// The LogMessage that is held by the LogEventsArgs object.
        /// </summary>
        public LogMessage LogMessage { get; private set; }

        /// <summary>
        /// Constructs a LogEventArgs object.
        /// </summary>
        /// <param name="logMessage">The LogMessage that should be held by the LogEventsArgs object.</param>
        public LogEventArgs(LogMessage logMessage)
        {
            LogMessage = logMessage;
        }
    }

    /// <summary>
    /// The Logger acts as a middleware between LoggingSections and LogHandlers.
    /// All logging traffic by LoggingSections goes through the Logger and is spread to subscribed LogHandlers by it.
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// LoggingSection used by the Logger itself.
        /// </summary>
        private static LoggingSection log = new LoggingSection("Logger");

        public static event EventHandler<LogEventArgs> LogHandlers;

        /// <summary>
        /// Handles logging.
        /// Called by LoggingSections.
        /// Not intended to be used manually. Use Wrappers provided by a LoggingSection.
        /// </summary>
        /// <param name="logMessage"></param>
        internal static void Log(LogMessage logMessage)
        {
            if (logMessage.LoggingSection.Disposed)
            {
                if (!log.Disposed)
                {
                    log.Warning($"Tried to log a {logMessage.LogLevel}-Message in LoggingSection {logMessage.LoggingSection.FullPath}, but it has already been disposed.");
                    return;
                }
            }

            OnLog(logMessage);
        }

        private static void OnLog(LogMessage logMessage)
        {
            if (LogHandlers != null)
                LogHandlers.Invoke(null, new LogEventArgs(logMessage));
        }
    }
}
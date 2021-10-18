using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HawkEye.Logging
{
    //TODO: Idea for future: LogHandlers
    //ILogHandler -> LogLevel defined and Output(); method
    //Implementations: ConsoleLogHandler (Outputs into console), FileLogHandler (Outputs into file)
    //LogHandlers will be registered in Logger
    //On log, Logger will loop through registered LogHandlers,
    // check if LogLevel is met, then call Output() function of LogHandler.

    public class LogEventArgs : EventArgs
    {
        public LogMessage LogMessage { get; private set; }

        public LogEventArgs(LogMessage logMessage)
        {
            LogMessage = logMessage;
        }
    }

    /// <summary>
    /// The Logger acts as a middleware between LoggingSections and LogHandlers.
    /// All logging traffic by LoggingSection goes through the Logger and is spread to the LogHandlers.
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
        /// Not intended to be used manually. Use Wrappers from LoggingSection.
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
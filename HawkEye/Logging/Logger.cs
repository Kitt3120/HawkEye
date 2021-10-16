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

    /// <summary>
    /// The Logger acts as a centralized point where all logging by LoggingSections is handled.
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// LoggingSection used by the Logger itself.
        /// </summary>
        private static LoggingSection log = new LoggingSection("Logger");

        /// <summary>
        /// List of enabled LogLevels.
        /// TODO: Remove after LogHandler implementation.
        /// </summary>
        private static List<LogLevel> enabledLevels { get; } = new List<LogLevel>((LogLevel[])Enum.GetValues(typeof(LogLevel)));

        /// <summary>
        /// Handles logging.
        /// Called by LoggingSections.
        /// Not intended to be used manually.
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

            //TODO: Replace with LogHandler implementation
            if (enabledLevels.Contains(logMessage.LogLevel))
                Console.WriteLine(logMessage.ToShortString());
        }
    }
}
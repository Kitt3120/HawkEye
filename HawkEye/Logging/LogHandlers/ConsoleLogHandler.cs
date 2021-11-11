using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HawkEye.Logging.LogHandlers
{
    /// <summary>
    /// A LogHandler that writes console
    /// </summary>
    public class ConsoleLogHandler : PreFormattedLogHandler
    {
        /// <summary>
        /// Constructs a ConsoleLogHandler
        /// </summary>
        /// <param name="format">Pattern according to which a LogMessage is converted into a string. If not provided, a default pattern will be used.</param>
        /// <param name="enabledLogLevels">Enabled LogLevels. If not provided, all LogLevels are enabled by default.</param>
        public ConsoleLogHandler(string format = null, LogLevel[] enabledLogLevels = null) : base(format, null, enabledLogLevels)
        { }

        /// <summary>
        /// Writes the converted LogMessage into console
        /// </summary>
        /// <param name="message">The string-converted LogMessage</param>
        public override void Output(string message)
        {
            Console.WriteLine(message);
        }
    }
}
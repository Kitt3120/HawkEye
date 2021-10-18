using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HawkEye.Logging.LogHandlers
{
    public class ConsoleLogHandler : LogHandler
    {
        public bool ShortFormat { get; set; }

        public ConsoleLogHandler(bool shortFormat, LogLevel[] enabledLogLevels = null) : base(enabledLogLevels)

        {
            ShortFormat = shortFormat;
        }

        public override void OnLog(object source, LogEventArgs eventArgs)
        {
            Console.WriteLine(ShortFormat ? eventArgs.LogMessage.ToShortString() : eventArgs.LogMessage.ToString());
        }
    }
}
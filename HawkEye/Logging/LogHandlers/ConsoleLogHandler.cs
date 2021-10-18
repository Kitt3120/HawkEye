using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HawkEye.Logging.LogHandlers
{
    public class ConsoleLogHandler : PreFormattedLogHandler
    {
        public ConsoleLogHandler(string format = null, LogLevel[] enabledLogLevels = null) : base(format, enabledLogLevels)
        { }

        public override void Output(string message)
        {
            Console.WriteLine(message);
        }
    }
}
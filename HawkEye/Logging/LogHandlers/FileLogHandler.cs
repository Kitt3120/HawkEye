using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HawkEye.Logging.LogHandlers
{
    internal class FileLogHandler : PreFormattedLogHandler
    {
        private StreamWriter streamWriter;

        public FileLogHandler(string path, string format = null, LogLevel[] enabledLogLevels = null) : base(format, enabledLogLevels)
        {
            using LoggingSection log = new LoggingSection(this);

            DirectoryInfo parent = Directory.GetParent(path);
            if (!parent.Exists)
            {
                log.Verbose($"Creating directory {parent.FullName}.");
                parent.Create();
            }

            try
            {
                streamWriter = new StreamWriter(File.Open(path, FileMode.OpenOrCreate, FileAccess.Write), Encoding.UTF8);
                streamWriter.AutoFlush = true;
            }
            catch (Exception e)
            {
                streamWriter = null;
                log.Error($"Failed to open FileStream on {path}: {e.Message}.");
            }
        }

        public override void Output(string message)
        {
            if (streamWriter != null)
                streamWriter.WriteLine(message);
        }
    }
}
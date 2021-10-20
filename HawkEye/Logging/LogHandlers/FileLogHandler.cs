using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HawkEye.Logging.LogHandlers
{
    /// <summary>
    /// A LogHandler that writes into a file
    /// </summary>
    internal class FileLogHandler : PreFormattedLogHandler, IDisposable
    {
        /// <summary>
        /// StreamWriter that is used for writing into the log file.
        /// </summary>
        private StreamWriter streamWriter;

        /// <summary>
        /// Constructs a FileLogHandler
        /// </summary>
        /// <param name="path">Path to the file that should be written into</param>
        /// <param name="format">Pattern according to which a LogMessage is converted into a string. If not provided, a default pattern will be used.</param>
        /// <param name="enabledLogLevels">Enabled LogLevels. If not provided, all LogLevels are enabled by default.</param>
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

        /// <summary>
        /// Writes the converted LogMessage into the log file
        /// </summary>
        /// <param name="message">The string-converted LogMessage</param>
        public override void Output(string message)
        {
            if (streamWriter != null)
                streamWriter.WriteLine(message);
        }

        /// <summary>
        /// Disposes the StreamWriter used by the FileLogHandler
        /// </summary>
        public void Dispose()
        {
            if (streamWriter != null)
            {
                streamWriter.Close();
                streamWriter = null;
            }
        }
    }
}
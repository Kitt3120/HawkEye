using HawkEye.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HawkEye.Scanning
{
    /// <summary>
    /// Registry of available scanners
    /// </summary>
    public class Scanners
    {
        private LoggingSection logging;
        private List<Scanner> scanners;

        public Scanners()
        {
            logging = new LoggingSection(this);
            scanners = new List<Scanner>();

            logging.Info("Initializing scanners");
            foreach (Type type in AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => !p.IsInterface && p != typeof(Scanner) && typeof(Scanner).IsAssignableFrom(p))
                .Where(t => t.GetConstructors().All(c => c.GetParameters().Length == 0)))
                scanners.Add((Scanner)Activator.CreateInstance(type));
        }

        public Scanner[] GetScanners(string file)
        {
            if (!File.Exists(file))
                throw new FileNotFoundException($"No file was found at the given path: {file}");
            return scanners.Where(scanner => scanner.IsSuitableFor(file)).ToArray();
        }
    }
}
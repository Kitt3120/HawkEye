using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HawkEye.Scanning
{
    /// <summary>
    /// Abstraction of scanner classes
    /// </summary>
    public abstract class Scanner
    {
        /// <summary>
        /// Name of scanner that can be displayed to the user.
        /// </summary>
        public abstract string FriendlyName { get; protected set; }

        /// <summary>
        /// Uses scanner on target file
        /// </summary>
        /// <param name="file">Target file to be scanned</param>
        /// <param name="cancellationToken">CancellationToken for requesting cancellation</param>
        /// <returns>Gathered data</returns>
        public abstract Task<string> ScanAsync(string file, CancellationToken cancellationToken);

        /// <summary>
        /// Checks if the scanner is suitable for the given target file.
        /// </summary>
        /// <param name="file">Target file to check suitability of scanner for</param>
        /// <returns>Whether or not the scanner is suitable for the target file</returns>
        public abstract bool IsSuitableFor(string file);
    }
}
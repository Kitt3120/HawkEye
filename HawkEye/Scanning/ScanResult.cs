using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HawkEye.Scanning
{
    public class ScanResult
    {
        /// <summary>
        /// DateTime of when the scan started
        /// </summary>
        public DateTime BeginOfScan { get; protected set; }

        /// <summary>
        /// DateTime of when the scan finished, failed or has been aborted
        /// </summary>
        public DateTime EndOfScan { get; protected set; }

        /// <summary>
        /// Calculates the timespan between begin and end of scan.
        /// If the scan has not started or finished yet, the timespan will be of 0 ticks.
        /// </summary>
        public TimeSpan TimeSpan { get => BeginOfScan.Ticks == 0 || EndOfScan.Ticks == 0 ? new TimeSpan(0) : EndOfScan - BeginOfScan; }

        /// <summary>
        /// The target file
        /// </summary>
        public string File { get; private set; }

        /// <summary>
        /// Information about the current state
        /// </summary>
        public ScanStatus Status { get; protected set; }

        /// <summary>
        /// The scanner used for scanning the target file
        /// </summary>
        public Scanner Scanner { get; protected set; }

        /// <summary>
        /// Information gathered by the scanner
        /// </summary>
        public string Content { get; protected set; }

        /// <summary>
        /// On failing, this is set to the encountered exception.
        /// Otherwise this is null.
        /// </summary>
        public Exception ThrownException { get; protected set; }

        //TODO: Add issuer property (User or issued through automated job)

        /// <summary>
        /// Creates a new ScanResult
        /// </summary>
        /// <param name="file">The target file</param>
        public ScanResult(string file)
        {
            File = file;
            Status = ScanStatus.Pending;
        }
    }
}
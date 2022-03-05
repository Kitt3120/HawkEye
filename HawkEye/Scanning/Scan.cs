using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HawkEye.Scanning
{
    /// <summary>
    /// Represents a file scan, whether pending, running or in a finished state.
    /// </summary>
    public class Scan
    {
        /// <summary>
        /// DateTime of when the scan started.
        /// </summary>
        public DateTime BeginOfScan { get; protected internal set; }

        /// <summary>
        /// DateTime of when the scan finished, failed or has been aborted.
        /// </summary>
        public DateTime EndOfScan { get; protected internal set; }

        /// <summary>
        /// Calculates the TimeSpan between begin and end of scan.
        /// If the scan has not started yet, the TimeSpan will be of 0 ticks.
        /// If the scan is running, returns the TimeSpan since start of scan.
        /// </summary>
        public TimeSpan TimeSpan
        {
            get
            {
                switch (Status)
                {
                    case ScanStatus.Pending:
                        return new TimeSpan(0);

                    case ScanStatus.Running:
                        return DateTime.Now - BeginOfScan;

                    default:
                        return EndOfScan - BeginOfScan;
                }
            }
        }

        /// <summary>
        /// Scanner that was used on the target file.
        /// </summary>
        public Scanner Scanner { get; private set; }

        /// <summary>
        /// The target file.
        /// </summary>
        public string File { get; private set; }

        /// <summary>
        /// Information about the current state of the scan.
        /// </summary>
        public ScanStatus Status { get; protected internal set; }

        /// <summary>
        /// Result data gathered by the scanner from the target file.
        /// </summary>
        public string Result { get; protected internal set; }

        /// <summary>
        /// On failing, this is set to the encountered exception.
        /// Otherwise this is null.
        /// </summary>
        public Exception ThrownException { get; protected internal set; }

        /// <summary>
        /// CancellationToken to give information about whether a cancellation has been requested.
        /// </summary>
        private CancellationToken cancellationToken;

        //TODO: Add issuer property (User or issued through automated job)

        /// <summary>
        /// Constructs a Scan.
        /// </summary>
        /// <param name="scanner">Scanner to scan target file</param>
        /// <param name="file">Target file</param>
        /// <param name="cancellationToken">CancellationToken for requesting cancellation</param>
        public Scan(Scanner scanner, string file, CancellationToken cancellationToken)
        {
            Scanner = scanner;
            File = file;
            this.cancellationToken = cancellationToken;
            Status = ScanStatus.Pending;
        }

        /// <summary>
        /// Scans the target file using the specified scanner.
        /// </summary>
        public async Task ScanAsync()
        {
            Status = ScanStatus.Running;
            BeginOfScan = DateTime.Now;

            if (cancellationToken.IsCancellationRequested)
            {
                Result = "Scan aborted before any data could be gathered.";
                Status = ScanStatus.Aborted;
            }
            else
            {
                try
                {
                    Result = await Scanner.ScanAsync(File, cancellationToken);
                    Status = cancellationToken.IsCancellationRequested ? ScanStatus.Aborted : ScanStatus.Finished;
                }
                catch (Exception e)
                {
                    Status = ScanStatus.Failed;
                    ThrownException = e;
                }
            }
            EndOfScan = DateTime.Now;
        }
    }
}
﻿using System;
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
        /// DateTime of when the scan was created.
        /// </summary>
        public DateTime TimeOfCreation { get; private set; }
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
        /// If the scan is pending, the TimeSpan will represent the pending duration.
        /// If the scan is running, returns the TimeSpan since start of scan.
        /// If the scan is finished, returns the TimeSpan from start to end of scan.
        /// </summary>
        public TimeSpan TimeSpan
        {
            get
            {
                switch (Status)
                {
                    case ScanStatus.Pending:
                        return DateTime.Now - TimeOfCreation;

                    case ScanStatus.Running:
                        return DateTime.Now - BeginOfScan;

                    case ScanStatus.Finished:
                    case ScanStatus.Aborted:
                    case ScanStatus.Failed:
                        return EndOfScan - BeginOfScan;
                    default: //Only possible when a new ScanStatus type has been added but not yet implemented here
                        return new TimeSpan(0);
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
        private readonly CancellationToken cancellationToken;

        //TODO: Add issuer property (Manually through user or issued through automated job)

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
            TimeOfCreation = DateTime.Now;
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
                Result = "Cancellation requested before any data could be gathered.";
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
                    Result = $"{e.Message} thrown while scanning.";
                    Status = ScanStatus.Failed;
                    ThrownException = e;
                }
            }
            EndOfScan = DateTime.Now;
        }
    }
}
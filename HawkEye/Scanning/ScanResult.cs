using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HawkEye.Scanning
{
    public class ScanResult
    {
        public DateTime BeginOfScan { get; protected set; }
        public DateTime EndOfScan { get; protected set; }
        public TimeSpan TimeSpan { get => BeginOfScan.Ticks == 0 || EndOfScan.Ticks == 0 ? new TimeSpan(0) : EndOfScan - BeginOfScan; }
        public ScanStatus Status { get; protected set; }
        public string File { get; private set; }
        public Scanner Scanner { get; protected set; }
        public string Content { get; protected set; }
        public Exception ThrownException { get; protected set; }

        //TODO: Add issuer property (User or issued through automated job)

        public ScanResult(string file)
        {
            File = file;
            Status = ScanStatus.Pending;
        }
    }
}
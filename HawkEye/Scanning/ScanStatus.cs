using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HawkEye.Scanning
{
    public enum ScanStatus
    {
        /// <summary>
        /// Indicates that a Scan is still pending and awaiting future execution
        /// </summary>
        Pending,

        /// <summary>
        /// Indicates that a Scan is currently running
        /// </summary>
        Running,

        /// <summary>
        /// Indicates that a Scan was pending or running and has been aborted
        /// </summary>
        Aborted,

        /// <summary>
        /// Indicates that a Scan has run into an exception and did not finish successfully
        /// </summary>
        Failed
    }
}
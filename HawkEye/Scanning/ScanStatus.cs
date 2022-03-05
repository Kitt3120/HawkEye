using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HawkEye.Scanning
{
    /// <summary>
    /// Indications for a scan's state
    /// </summary>
    public enum ScanStatus
    {
        /// <summary>
        /// Indicates that a scan is still pending and awaiting future execution
        /// </summary>
        Pending,

        /// <summary>
        /// Indicates that a scan is currently running
        /// </summary>
        Running,

        /// <summary>
        /// Indicates that a scan is finished
        /// </summary>
        Finished,

        /// <summary>
        /// Indicates that a scan was pending or running and has been aborted
        /// </summary>
        Aborted,

        /// <summary>
        /// Indicates that a scan has run into an exception and did not finish successfully
        /// </summary>
        Failed
    }
}
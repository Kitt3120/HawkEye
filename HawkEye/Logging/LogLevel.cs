using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HawkEye.Logging
{
    /// <summary>
    /// The different types of levels to be used for logging.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// For when the error will prevent the program from running at all or behaving as expected.
        /// </summary>
        CRITICAL,

        /// <summary>
        /// For when the error has to be handled by alternative code but the program can still go on.
        /// </summary>
        ERROR,

        /// <summary>
        /// For minor errors that do not need to be handled, just noted.
        /// </summary>
        WARNING,

        /// <summary>
        /// For general messages.
        /// </summary>
        INFO,

        /// <summary>
        /// For more in-depth messages.
        /// </summary>
        VERBOSE,

        /// <summary>
        /// For most detailed messages.
        /// </summary>
        DEBUG
    }
}
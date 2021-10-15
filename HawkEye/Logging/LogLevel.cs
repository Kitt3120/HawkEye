using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HawkEye.Logging
{
    public enum LogLevel
    {
        CRITICAL,   //For when the error will prevent the program from running at all or behaving as expected.
        ERROR,      //For when the error has to be handled by alternative code but the program can still go on.
        WARNING,    //For minor errors that do not need to be handled, just noted
        INFO,       //For general messages
        VERBOSE,    //For more in-depth messages
        DEBUG       //For most detailed messages
    }
}
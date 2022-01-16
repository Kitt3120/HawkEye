using HawkEye.Logging;
using HawkEye.Logging.LogHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HawkEye.UnitTests
{
    /// <summary>
    /// This class contains Unit Tests for the Logger class of the Logging Framework.
    /// </summary>
    public class LoggerTests
    {
        /// <summary>
        /// This test checks that registered LogHandlers are being called by Logger.Log().
        /// </summary>
        [Fact]
        public void Log_ShouldTriggerLogHandlers()
        {
            //Arrange
            bool hasLogged = false;
            EventHandler<LogEventArgs> logHandler = (source, args) =>
            {
                hasLogged = true;
            };
            LoggingSection loggingSection = new LoggingSection(this);

            //Act
            Logger.LogHandlers += logHandler;
            loggingSection.Info("Test");

            //Assert
            Assert.True(hasLogged);
        }
    }
}
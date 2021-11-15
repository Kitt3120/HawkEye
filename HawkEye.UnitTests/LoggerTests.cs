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
    public class LoggerTests
    {
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
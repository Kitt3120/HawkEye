using HawkEye.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HawkEye.UnitTests
{
    public class LogMessageTests
    {
        [Fact]
        public void LogMessage_ShouldHaveCorrectLoggingSection()
        {
            //Arrange
            LoggingSection loggingSection = new LoggingSection(this);
            LogMessage logMessage;

            //Act
            logMessage = loggingSection.Log(LogLevel.INFO, "Test");

            //Assert
            Assert.NotNull(logMessage);
            Assert.Equal(loggingSection, logMessage.LoggingSection);
        }

        [Fact]
        public void LogMessage_ShouldHaveCorrectMessage()
        {
            //Arrange
            LoggingSection loggingSection = new LoggingSection(this);
            LogMessage logMessage;
            string message = "Test";

            //Act
            logMessage = loggingSection.Log(LogLevel.INFO, message);

            //Assert
            Assert.NotNull(logMessage);
            Assert.Equal(message, logMessage.Message);
        }

        [Fact]
        public void LogMessage_ShouldHaveCorrectTimestamp()
        {
            //Arrange
            LoggingSection loggingSection = new LoggingSection(this);
            LogMessage logMessage;
            DateTime now;

            //Act
            logMessage = loggingSection.Info("Test");
            now = DateTime.Now;

            //Assert
            Assert.NotNull(logMessage);
            //TODO: Remove out-commented code if below solution proves to work consistently
            //Assert.Equal(now.Minute, logMessage.Timestamp.Minute);
            //Assert.Equal(now.Hour, logMessage.Timestamp.Hour);
            //Assert.Equal(now.Day, logMessage.Timestamp.Day);
            //Assert.Equal(now.Month, logMessage.Timestamp.Month);
            //Assert.Equal(now.Year, logMessage.Timestamp.Year);
            Assert.True(now.Ticks / TimeSpan.TicksPerMillisecond - logMessage.Timestamp.Ticks / TimeSpan.TicksPerMillisecond < 1000);
        }
    }
}
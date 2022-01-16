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
    /// This is a dummy LogHandler used by the Unit Tests
    /// </summary>
    public class TestLogHandler : LogHandler
    {
        public bool Triggered { get; private set; }

        public TestLogHandler(LogLevel[] enabledLogLevels = null) : base(enabledLogLevels)
        {
            Triggered = false;
        }

        public override void HandleLogMessage(LogMessage logMessage)
        {
            Triggered = true;
        }
    }

    /// <summary>
    /// This class contains Unit Tests for the LogHandler class of the Logging Framework.
    /// </summary>
    public class LogHandlerTests
    {
        /// <summary>
        /// This test checks that a LogHandler has all LogLevels enabled by default.
        /// </summary>
        [Fact]
        public void LogHandler_ShouldHaveAllLevelsEnabledByDefault()
        {
            //Arrange
            TestLogHandler testLogHandler = new TestLogHandler();
            List<LogLevel> listOfAllLogLevels = new List<LogLevel>((LogLevel[])Enum.GetValues(typeof(LogLevel)));
            IReadOnlyCollection<LogLevel> testLogHandlerLogLevels = testLogHandler.GetEnabledLogLevels();

            //Act

            //Assert
            Assert.NotNull(testLogHandler.GetEnabledLogLevels());
            Assert.Equal(listOfAllLogLevels.Count, testLogHandlerLogLevels.Count);
            Assert.False(listOfAllLogLevels.Except(testLogHandlerLogLevels).Any()); //Subtracting both lists should result in an empty list. If not, it means that one of the lists contained a LogLevel multiple times, which is an invalid state.
        }

        /// <summary>
        /// This test checks that a LogHandler has all LogLevels enabled that have been passed to the constructer.
        /// </summary>
        [Fact]
        public void LogHandler_ShouldCopyProvidedArrayOfEnabledLogLevels()
        {
            //Arrange
            LogLevel[] logLevels1 = Enum.GetValues<LogLevel>();
            LogLevel[] logLevels2 = new LogLevel[] { LogLevel.INFO };
            LogLevel[] logLevels3 = new LogLevel[0];

            TestLogHandler testLogHandler1 = new TestLogHandler(logLevels1);
            TestLogHandler testLogHandler2 = new TestLogHandler(logLevels2);
            TestLogHandler testLogHandler3 = new TestLogHandler(logLevels3);

            IReadOnlyCollection<LogLevel> testLogHandlerLogLevels1 = testLogHandler1.GetEnabledLogLevels();
            IReadOnlyCollection<LogLevel> testLogHandlerLogLevels2 = testLogHandler2.GetEnabledLogLevels();
            IReadOnlyCollection<LogLevel> testLogHandlerLogLevels3 = testLogHandler3.GetEnabledLogLevels();

            //Act

            //Assert
            Assert.NotNull(testLogHandler1.GetEnabledLogLevels());
            Assert.NotNull(testLogHandler2.GetEnabledLogLevels());
            Assert.NotNull(testLogHandler3.GetEnabledLogLevels());
            Assert.Equal(logLevels1.Length, testLogHandlerLogLevels1.Count);
            Assert.Equal(logLevels2.Length, testLogHandlerLogLevels2.Count);
            Assert.Equal(logLevels3.Length, testLogHandlerLogLevels3.Count);
            Assert.False(logLevels1.Except(testLogHandlerLogLevels1).Any());
            Assert.False(logLevels2.Except(testLogHandlerLogLevels2).Any());
            Assert.False(logLevels3.Except(testLogHandlerLogLevels3).Any());
        }

        /// <summary>
        /// This test checks that a LogLevel is being enabled by Enable().
        /// </summary>
        [Fact]
        public void Enable_ShouldEnableLogLevel()
        {
            //Arrange
            TestLogHandler testLogHandler = new TestLogHandler(new LogLevel[0]);

            //Act
            testLogHandler.Enable(LogLevel.INFO);

            //Assert
            Assert.True(testLogHandler.IsEnabled(LogLevel.INFO));
        }

        /// <summary>
        /// This test checks that a LogLevel is being disabled by Disable().
        /// </summary>
        [Fact]
        public void Disable_ShouldDisableLogLevel()
        {
            //Arrange
            TestLogHandler testLogHandler = new TestLogHandler(new LogLevel[] { LogLevel.INFO });

            //Act
            testLogHandler.Disable(LogLevel.INFO);

            //Assert
            Assert.False(testLogHandler.IsEnabled(LogLevel.INFO));
        }

        /// <summary>
        /// This test checks that a LogHandler is being triggered by a LogMessage of an enabled LogLevel.
        /// </summary>
        [Fact]
        public void LogHandler_ShouldTriggerIfLogLevelEnabled()
        {
            //Arrange
            TestLogHandler testLogHandler = new TestLogHandler(new LogLevel[] { LogLevel.INFO });
            Logger.LogHandlers += testLogHandler.OnLog;
            LoggingSection loggingSection = new LoggingSection(this);

            //Act
            loggingSection.Info("Test");

            //Assert
            Assert.True(testLogHandler.Triggered);
        }

        /// <summary>
        /// This test checks that a LogHandler isn't being triggered by a LogMessage of a disabled LogLevel.
        /// </summary>
        [Fact]
        public void LogHandler_ShouldNotTriggerIfLogLevelDisabled()
        {
            //Arrange
            TestLogHandler testLogHandler = new TestLogHandler(new LogLevel[0]);
            Logger.LogHandlers += testLogHandler.OnLog;
            LoggingSection loggingSection = new LoggingSection(this);

            //Act
            loggingSection.Info("Test");

            //Assert
            Assert.False(testLogHandler.Triggered);
        }
    }
}
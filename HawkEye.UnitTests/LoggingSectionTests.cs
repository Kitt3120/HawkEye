using HawkEye.Logging;
using System;
using System.Collections.Generic;
using Xunit;

namespace HawkEye.UnitTests
{
    /// <summary>
    /// This class contains Unit Tests for the LoggingSection class of the Logging Framework.
    /// </summary>
    public class LoggingSectionTests
    {
        /// <summary>
        /// This test checks that a LoggingSection is not initially marked as disposed.
        /// </summary>
        [Fact]
        public void Disposed_ShouldBeFalseAfterConstruction()
        {
            //Arrange
            LoggingSection loggingSection = new LoggingSection(this);

            //Act

            //Assert
            Assert.False(loggingSection.Disposed);
        }

        /// <summary>
        /// This test checks that a LoggingSection is marked as disposed after calling Dispose() on it.
        /// </summary>
        [Fact]
        public void Disposed_ShouldBeTrueAfterDisposal()
        {
            //Arrange
            LoggingSection loggingSection = new LoggingSection(this);

            //Act
            loggingSection.Dispose();

            //Assert
            Assert.True(loggingSection.Disposed);
        }

        /// <summary>
        /// This test checks that GetLogMessages() returns null when used on a disposed LoggingSection.
        /// </summary>
        [Fact]
        public void GetLogMessages_ShouldReturnNullAfterDisposal()
        {
            //Arrange
            LoggingSection loggingSection = new LoggingSection(this);

            //Act
            loggingSection.Dispose();

            //Assert
            Assert.Null(loggingSection.GetLogMessages());
        }

        /// <summary>
        /// This test checks that GetChildren() returns null when used on a disposed LoggingSection.
        /// </summary>
        [Fact]
        public void GetChildren_ShouldReturnNullAfterDisposal()
        {
            //Arrange
            LoggingSection loggingSection = new LoggingSection(this);

            //Act
            loggingSection.Dispose();

            //Assert
            Assert.Null(loggingSection.GetChildren());
        }

        /// <summary>
        /// This test checks that a LoggingSection's list of children is initally empty.
        /// </summary>
        [Fact]
        public void Children_ShouldBeEmptyAfterConstruction()
        {
            //Arrange
            LoggingSection loggingSection = new LoggingSection(this);

            //Act

            //Assert
            Assert.NotNull(loggingSection.GetChildren());
            Assert.True(loggingSection.GetChildren().Count == 0);
        }

        /// <summary>
        /// This test checks that a LoggingSection's list of LogMessages is initally empty.
        /// </summary>
        [Fact]
        public void LogMessages_ShouldBeEmptyAfterConstruction()
        {
            //Arrange
            LoggingSection loggingSection = new LoggingSection(this);

            //Act

            //Assert
            Assert.NotNull(loggingSection.GetLogMessages());
            Assert.True(loggingSection.GetLogMessages().Count == 0);
        }

        /// <summary>
        /// This test checks that a LoggingSection's list of LogMessages is not empty after logging.
        /// </summary>
        [Fact]
        public void LogMessages_ShouldHaveOneEntryAfterLogging()
        {
            //Arrange
            LoggingSection loggingSection = new LoggingSection(this);

            //Act
            loggingSection.Info("Test");

            //Assert
            Assert.NotNull(loggingSection.GetLogMessages());
            Assert.True(loggingSection.GetLogMessages().Count == 1);
        }

        /// <summary>
        /// This test checks that children register themselves in their parents' list of children.
        /// </summary>
        [Fact]
        public void CreateChild_ShouldAddChildToChildrenList()
        {
            //Arrange
            LoggingSection loggingSection = new LoggingSection(this);

            //Act
            LoggingSection child1 = loggingSection.CreateChild(this);
            LoggingSection child2 = loggingSection.CreateChild(this);
            IReadOnlyCollection<LoggingSection> children = loggingSection.GetChildren();

            //Assert
            Assert.Equal(2, children.Count);
            Assert.Contains(child1, children);
            Assert.Contains(child2, children);
        }

        /// <summary>
        /// This test checks that children set their parent LoggingSections correctly.
        /// </summary>
        [Fact]
        public void CreateChild_ParentShouldBeSetInChild()
        {
            //Arrange
            LoggingSection loggingSection = new LoggingSection(this);

            //Act
            LoggingSection child = loggingSection.CreateChild(this);

            //Assert
            Assert.Equal(loggingSection, child.Parent);
        }

        /// <summary>
        /// This test checks that the LoggingSection constructor sets the correct name from a given string.
        /// </summary>
        [Fact]
        public void LoggingSection_CorrectNameShouldBeSetFromString()
        {
            //Arrange
            LoggingSection loggingSection = new LoggingSection("Test");

            //Act

            //Assert
            Assert.Equal("Test", loggingSection.Name);
        }

        /// <summary>
        /// This test checks that the LoggingSection constructor sets the correct name from a given object without generic types.
        /// </summary>
        [Fact]
        public void LoggingSection_CorrectNameShouldBeSetFromObject()
        {
            //Arrange
            LoggingSection loggingSection = new LoggingSection(this);

            //Act

            //Assert
            Assert.Equal("LoggingSectionTests", loggingSection.Name);
        }

        /// <summary>
        /// This test checks that the LoggingSection constructor sets the correct name from a given object with a generic type.
        /// </summary>
        [Fact]
        public void LoggingSection_CorrectNameShouldBeSetFromObjectWithGenericType()
        {
            //Arrange
            List<object> list = new List<object>();
            LoggingSection loggingSection = new LoggingSection(list);

            //Act
            loggingSection.Info("Test");

            //Assert
            Assert.Equal("List`1<Object>", loggingSection.Name);
        }

        /// <summary>
        /// This test checks that the produced LogMessage by Debug() has the Debug level
        /// </summary>
        [Fact]
        public void Log_ReturnedLogMessage_ShouldHaveLogLevelDebug()
        {
            //Arrange
            LoggingSection loggingSection = new LoggingSection(this);
            LogMessage logMessage;

            //Act
            logMessage = loggingSection.Debug("Test");

            //Assert
            Assert.NotNull(logMessage);
            Assert.Equal(LogLevel.DEBUG, logMessage.LogLevel);
        }

        /// <summary>
        /// This test checks that the produced LogMessage by Verbose() has the Verbose level
        /// </summary>
        [Fact]
        public void Log_ReturnedLogMessage_ShouldHaveLogLevelVerbose()
        {
            //Arrange
            LoggingSection loggingSection = new LoggingSection(this);
            LogMessage logMessage;

            //Act
            logMessage = loggingSection.Verbose("Test");

            //Assert
            Assert.NotNull(logMessage);
            Assert.Equal(LogLevel.VERBOSE, logMessage.LogLevel);
        }

        /// <summary>
        /// This test checks that the produced LogMessage by Info() has the Info level
        /// </summary>
        [Fact]
        public void Log_ReturnedLogMessage_ShouldHaveLogLevelInfo()
        {
            //Arrange
            LoggingSection loggingSection = new LoggingSection(this);
            LogMessage logMessage;

            //Act
            logMessage = loggingSection.Info("Test");

            //Assert
            Assert.NotNull(logMessage);
            Assert.Equal(LogLevel.INFO, logMessage.LogLevel);
        }

        /// <summary>
        /// This test checks that the produced LogMessage by Warning() has the Warning level
        /// </summary>
        [Fact]
        public void Log_ReturnedLogMessage_ShouldHaveLogLevelWarning()
        {
            //Arrange
            LoggingSection loggingSection = new LoggingSection(this);
            LogMessage logMessage;

            //Act
            logMessage = loggingSection.Warning("Test");

            //Assert
            Assert.NotNull(logMessage);
            Assert.Equal(LogLevel.WARNING, logMessage.LogLevel);
        }

        /// <summary>
        /// This test checks that the produced LogMessage by Error() has the Error level
        /// </summary>
        [Fact]
        public void Log_ReturnedLogMessage_ShouldHaveLogLevelError()
        {
            //Arrange
            LoggingSection loggingSection = new LoggingSection(this);
            LogMessage logMessage;

            //Act
            logMessage = loggingSection.Error("Test");

            //Assert
            Assert.NotNull(logMessage);
            Assert.Equal(LogLevel.ERROR, logMessage.LogLevel);
        }

        /// <summary>
        /// This test checks that the produced LogMessage by Critical() has the Critical level
        /// </summary>
        [Fact]
        public void Log_ReturnedLogMessage_ShouldHaveLogLevelCritical()
        {
            //Arrange
            LoggingSection loggingSection = new LoggingSection(this);
            LogMessage logMessage;

            //Act
            logMessage = loggingSection.Critical("Test");

            //Assert
            Assert.NotNull(logMessage);
            Assert.Equal(LogLevel.CRITICAL, logMessage.LogLevel);
        }
    }
}
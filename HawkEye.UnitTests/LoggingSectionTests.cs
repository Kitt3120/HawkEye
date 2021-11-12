using HawkEye.Logging;
using System;
using System.Collections.Generic;
using Xunit;

namespace HawkEye.UnitTests
{
    public class LoggingSectionTests
    {
        [Fact]
        public void LoggingSection_ShouldBeUndisposedAfterConstruction()
        {
            //Arrange
            LoggingSection loggingSection = new LoggingSection(this);

            //Act

            //Assert
            Assert.False(loggingSection.Disposed);
        }

        [Fact]
        public void Dispose_ShouldBeTrueAfterDisposal()
        {
            //Arrange
            LoggingSection loggingSection = new LoggingSection(this);

            //Act
            loggingSection.Dispose();

            //Assert
            Assert.True(loggingSection.Disposed);
        }

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

        [Fact]
        public void GetChildren_ShouldReturnNullAfterDisposal()
        {
            //Arrange
            LoggingSection loggingSection = new LoggingSection(this);

            //Act
            loggingSection.Dispose();

            //Assert
            Assert.Null(loggingSection.GetLogMessages());
        }

        [Fact]
        public void Children_ShouldBeEmptyListAfterConstruction()
        {
            //Arrange
            LoggingSection loggingSection = new LoggingSection(this);

            //Act

            //Assert
            Assert.NotNull(loggingSection.GetChildren());
            Assert.True(loggingSection.GetChildren().Count == 0);
        }

        [Fact]
        public void LogMessages_ShouldBeEmptyListAfterConstruction()
        {
            //Arrange
            LoggingSection loggingSection = new LoggingSection(this);

            //Act

            //Assert
            Assert.NotNull(loggingSection.GetLogMessages());
            Assert.True(loggingSection.GetLogMessages().Count == 0);
        }

        [Fact]
        public void CreateChild_ShouldAddChildToList()
        {
            //Arrange
            LoggingSection loggingSection = new LoggingSection(this);

            //Act
            new LoggingSection(this, loggingSection);

            //Assert
            Assert.Equal(1, loggingSection.GetChildren().Count);

            //Act
            new LoggingSection(this, loggingSection);

            //Assert
            Assert.Equal(2, loggingSection.GetChildren().Count);
        }

        [Fact]
        public void CreateChild_ParentIsSetInChild()
        {
            //Arrange
            LoggingSection loggingSection = new LoggingSection(this);

            //Act
            LoggingSection child = new LoggingSection(this, loggingSection);

            //Assert
            Assert.Equal(loggingSection, child.Parent);
        }

        [Fact]
        public void CreateChild_ParentContainsChild()
        {
            //Arrange
            LoggingSection loggingSection = new LoggingSection(this);

            //Act
            LoggingSection child = new LoggingSection(this, loggingSection);

            //Assert
            Assert.Contains(child, loggingSection.GetChildren());
        }

        [Fact]
        public void LoggingSection_CorrectNameIsSetFromString()
        {
            //Arrange
            LoggingSection loggingSection = new LoggingSection("Test");

            //Act

            //Assert
            Assert.Equal("Test", loggingSection.Name);
        }

        [Fact]
        public void LoggingSection_CorrectNameIsSetFromObject()
        {
            //Arrange
            LoggingSection loggingSection = new LoggingSection(this);

            //Act

            //Assert
            Assert.Equal("LoggingSectionTests", loggingSection.Name);
        }

        [Fact]
        public void LoggingSection_CorrectNameIsSetFromObjectWithGenericType()
        {
            //Arrange
            List<object> list = new List<object>();
            LoggingSection loggingSection = new LoggingSection(list);

            //Act
            loggingSection.Info("Test");

            //Assert
            Assert.Equal("List`1<Object>", loggingSection.Name);
        }
    }
}
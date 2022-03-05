using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HawkEye.Logging
{
    /// <summary>
    /// A LoggingSection can be used for logging across a certain part of the program.
    /// Different logical parts of the program should not share a logging section.
    /// Sub-Processes of a logical operation may use Child-Sections.
    /// </summary>
    public class LoggingSection : IDisposable
    {
        /// <summary>
        /// This is the name of the LoggingSection.
        /// It may is used when printing LogMessages.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The dispose status of the LoggingSection.
        /// If the LoggingSection has ben disposed, it is in an unusable state and mustn't be used any further.
        /// </summary>
        public bool Disposed { get; private set; }

        /// <summary>
        /// The LoggingSection's parent LoggingSection.
        /// Can be null. Check with HasParent.
        /// </summary>
        public LoggingSection Parent { get; private set; } = null;

        /// <summary>
        /// Whether or not the LoggingSection has a parent LoggingSection and is one of its children.
        /// </summary>
        public bool HasParent
        { get { return !Disposed && Parent != null && !Parent.Disposed; } }

        /// <summary>
        /// List of registered child LoggingSections.
        /// This list is being modified by the LoggingSection object itself only and mustn't be modified otherwise.
        /// </summary>
        private List<LoggingSection> children;

        /// <summary>
        /// Whether or not the LoggingSection has child LoggingSections.
        /// </summary>
        public bool HasChildren
        { get { return !Disposed && children != null && children.Any(); } }

        /// <summary>
        /// Used for thread-safe modification of the children list.
        /// </summary>
        private object childrenLock;

        /// <summary>
        /// List of generated LogMessages.
        /// This list is being modified by the LoggingSection object itself only and mustn't be modified otherwise.
        /// </summary>
        private List<LogMessage> logMessages;

        /// <summary>
        /// Whether or not the LoggingSection has LogMessages.
        /// </summary>
        public bool HasMessages
        { get { return !Disposed && logMessages != null && logMessages.Any(); } }

        /// <summary>
        /// Used for thread-safe modification of the messages list.
        /// </summary>
        private object logMessagesLock;

        /// <summary>
        /// The full path of the LoggingSection, combining all parents' names recursively and
        /// </summary>
        public string FullPath
        {
            get
            {
                //TODO: Potential for performance improvements through caching
                string path = Name;
                LoggingSection current = this;
                while (current.HasParent)
                {
                    current = current.Parent;
                    path = $"{current.Name}/{path}";
                }
                return path;
            }
        }

        /// <summary>
        /// Constructs a LoggingSection.
        /// </summary>
        /// <param name="obj">Object that should provide the name of the LoggingSection</param>
        public LoggingSection(object obj) : this(obj.GetType().Name + (obj.GetType().GenericTypeArguments.Length > 0 ? $"<{string.Join(", ", obj.GetType().GenericTypeArguments.Select(type => type.Name))}>" : ""))
        { }

        /// <summary>
        /// Constructs a LoggingSection.
        /// </summary>
        /// <param name="name">Name of the LoggingSection</param>
        public LoggingSection(string name)
        {
            Name = name;

            children = new List<LoggingSection>();
            logMessages = new List<LogMessage>();

            childrenLock = new object();
            logMessagesLock = new object();

            Disposed = false;
        }

        /// <summary>
        /// Creates a LoggingSection, sets its parent to this LoggingSection and registers the child.
        /// </summary>
        /// <param name="obj">Object that should provide the name of the LoggingSection</param>
        /// <returns>LoggingSection that has this LoggingSection set as parent.</returns>
        public LoggingSection CreateChild(object obj) => CreateChild(obj.GetType().Name + (obj.GetType().GenericTypeArguments.Length > 0 ? $"<{string.Join(", ", obj.GetType().GenericTypeArguments.Select(type => type.Name))}>" : ""));

        /// <summary>
        /// Creates a LoggingSection, sets its parent to this LoggingSection and registers the child.
        /// </summary>
        /// <param name="name">Name of the LoggingSection</param>
        /// <returns>LoggingSection that has this LoggingSection set as parent.</returns>
        public LoggingSection CreateChild(string name)
        {
            LoggingSection child = new LoggingSection(name);
            child.Parent = this;
            lock (childrenLock)
                children.Add(child);
            return child;
        }

        /// <summary>
        /// Generates a LogMessage for this LoggingSection.
        /// Still works after LoggingSection has been disposed, but LogMessage will not be added to the message list.
        /// </summary>
        /// <param name="logLevel">The LogLevel to be used for the LogMessage</param>
        /// <param name="message">The message to be used for the LogMessage</param>
        /// <returns>The generated LogMessage object.</returns>
        public LogMessage CreateLogMessage(LogLevel logLevel, string message)
        {
            LogMessage logMessage = new LogMessage(this, logLevel, message);
            if (!Disposed)
                lock (logMessagesLock)
                    logMessages.Add(logMessage);

            return logMessage;
        }

        /// <summary>
        /// Wraps the LoggingSection's LogMessages in a read-only wrapper.
        /// </summary>
        /// <returns>Read-only Collection of the LogMessages contained in the LoggingSection or null if LoggingSection was disposed.</returns>
        public IReadOnlyCollection<LogMessage> GetLogMessages()
        {
            if (Disposed)
                return null;

            return logMessages.AsReadOnly();
        }

        /// <summary>
        /// Wraps the LoggingSection's Children in a read-only wrapper.
        /// </summary>
        /// <returns>Read-only Collection of the LoggingSection's Children or null if LoggingSection was disposed.</returns>
        public IReadOnlyCollection<LoggingSection> GetChildren()
        {
            if (Disposed)
                return null;

            return children.AsReadOnly();
        }

        /// <summary>
        /// Disposes the LoggingSection and its children.
        /// The LoggingSection is in an unusable state afterwards.
        /// </summary>
        public void Dispose()
        {
            if (Disposed)
                return;

            for (int i = children.Count - 1; i >= 0; i--)
                children[i].Dispose();

            if (HasParent)
                lock (Parent.childrenLock)
                    Parent.children.Remove(this);

            Disposed = true;
        }

        //Wrappers for performing actual logging
        public LogMessage Debug(string message) => Log(LogLevel.DEBUG, message);

        public LogMessage Verbose(string message) => Log(LogLevel.VERBOSE, message);

        public LogMessage Info(string message) => Log(LogLevel.INFO, message);

        public LogMessage Warning(string message) => Log(LogLevel.WARNING, message);

        public LogMessage Error(string message) => Log(LogLevel.ERROR, message);

        public LogMessage Critical(string message) => Log(LogLevel.CRITICAL, message);

        public LogMessage Log(LogLevel logLevel, string message)
        {
            LogMessage logMessage = CreateLogMessage(logLevel, message);
            Logger.Log(logMessage);
            return logMessage;
        }
    }
}
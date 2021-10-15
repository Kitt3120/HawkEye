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
        public bool HasParent { get { return Parent != null; } }

        /// <summary>
        /// List of registered child LoggingSections.
        /// This list is being modified by the LoggingSection object itself only and mustn't be modified otherwise.
        /// </summary>
        private List<LoggingSection> children;

        /// <summary>
        /// Whether or not the LoggingSection has child LoggingSections.
        /// </summary>
        public bool HasChildren { get { return children != null && children.Any(); } }

        /// <summary>
        /// Used for thread-safe modification of the children list.
        /// </summary>
        private object childrenLock;

        /// <summary>
        /// List of generated LogMessages.
        /// This list is being modified by the LoggingSection object itself only and mustn't be modified otherwise.
        /// </summary>
        private List<LogMessage> messages;

        /// <summary>
        /// Whether or not the LoggingSection has messages.
        /// </summary>
        public bool HasMessages { get { return messages != null && messages.Any(); } }

        /// <summary>
        /// Used for thread-safe modification of the messages list.
        /// </summary>
        private object messagesLock;

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
        /// <param name="obj">Object that should provide the name</param>
        /// <param name="parent">Parent LoggingSection</param>
        public LoggingSection(object obj, LoggingSection parent = null) : this(obj.GetType().Name + (obj.GetType().GenericTypeArguments.Length > 0 ? $"<{string.Join(", ", obj.GetType().GenericTypeArguments.Select(type => type.Name))}>" : ""), parent)
        { }

        /// <summary>
        /// Constructs a LoggingSection.
        /// </summary>
        /// <param name="name">Name of the LoggingSection</param>
        /// <param name="parent">Parent LoggingSection</param>
        public LoggingSection(string name, LoggingSection parent = null)
        {
            Name = name;

            if (parent != null)
            {
                lock (parent.childrenLock)
                {
                    Parent = parent;
                    parent.children.Add(this);
                }
            }

            children = new List<LoggingSection>();
            messages = new List<LogMessage>();

            childrenLock = new object();
            messagesLock = new object();

            Disposed = false;
        }

        /// <summary>
        /// Generates a LogMessage for this LoggingSection.
        /// </summary>
        /// <param name="logLevel">The LogLevel to be used for the LogMessage</param>
        /// <param name="message">The message to be used for the LogMessage</param>
        /// <returns>The generated LogMessage object or null if LoggingSection was disposed.</returns>
        public LogMessage Log(LogLevel logLevel, string message)
        {
            if (Disposed)
                return null; //TODO: Throw exception

            LogMessage logMessage = new LogMessage(this, logLevel, message);
            lock (messagesLock)
                messages.Add(logMessage);
            return logMessage;
        }

        /// <summary>
        /// Disposes the LoggingSection and its children.
        /// </summary>
        public void Dispose()
        {
            if (Disposed)
                return;
            while (!children.Any())
                children.First().Dispose();

            if (HasParent)
                lock (Parent.childrenLock)
                    Parent.children.Remove(this);

            Parent = null;
            messages = null;
            Disposed = true;
        }

        //Wrappers
        //public void Debug(string message) => Logger.Log(new LogMessage(this, LogLevel.Debug, message, DateTime.Now));
        //public void Verbose(string message) => Logger.Log(new LogMessage(this, LogLevel.Verbose, message, DateTime.Now));
        //public void Info(string message) => Logger.Log(new LogMessage(this, LogLevel.Info, message, DateTime.Now));
        //public void Warning(string message) => Logger.Log(new LogMessage(this, LogLevel.Warning, message, DateTime.Now));
        //public void Error(string message) => Logger.Log(new LogMessage(this, LogLevel.Error, message, DateTime.Now));
        //public void Critical(string message) => Logger.Log(new LogMessage(this, LogLevel.Critical, message, DateTime.Now));
        //public void Log(LogLevel logLevel, string message) => Logger.Log(new LogMessage(this, logLevel, message, DateTime.Now));
    }
}
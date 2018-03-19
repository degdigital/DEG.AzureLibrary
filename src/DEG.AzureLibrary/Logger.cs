using Microsoft.Azure.WebJobs.Host;

namespace DEG.AzureLibrary
{
    /// <summary>
    /// Interface ILog
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// Informations the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>ILog.</returns>
        ILog Info(string message);
        /// <summary>
        /// Warns the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>ILog.</returns>
        ILog Warn(string message);
        /// <summary>
        /// Fatals the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>ILog.</returns>
        ILog Fatal(string message);
    }

    //public class Logger
    //{
    //    readonly ILogger _log;
    //    readonly string _invocationId;
    //    //readonly LogLevel _logLevel;

    //    public Logger(string invocationId, ILogger logger)
    //    {
    //        _log = logger;
    //        _invocationId = invocationId;
    //    }

    //    public Logger Debug(string message)
    //    {
    //        _log.LogDebug("{invocationId}: {message}", _invocationId, message);
    //        return this;
    //    }

    //    public Logger Info(string message)
    //    {
    //        _log.LogInformation("{invocationId}: {message}", _invocationId, message);
    //        return this;
    //    }

    //    public Logger Warn(string message)
    //    {
    //        _log.LogWarning("{invocationId}: {message}", _invocationId, message);
    //        return this;
    //    }

    //    public Logger Error(string message)
    //    {
    //        _log.LogError("{invocationId}: {message}", _invocationId, message);
    //        return this;
    //    }

    //    public Logger Critical(string message)
    //    {
    //        _log.LogCritical("{invocationId}: {message}", _invocationId, message);
    //        return this;
    //    }
    //}

    /// <summary>
    /// Class TraceLogger.
    /// </summary>
    /// <seealso cref="DEG.AzureLibrary.ILog" />
    public class TraceLogger : ILog
    {
        readonly TraceWriter _log;
        readonly string _invocationId;

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceLogger"/> class.
        /// </summary>
        /// <param name="invocationId">The invocation identifier.</param>
        /// <param name="log">The log.</param>
        public TraceLogger(string invocationId, TraceWriter log)
        {
            _log = log;
            _invocationId = invocationId;
        }

        /// <summary>
        /// Informations the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>ILog.</returns>
        public ILog Info(string message)
        {
            _log.Info($"({_invocationId}): {message}");
            _log.Flush();
            return this;
        }

        /// <summary>
        /// Warns the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>ILog.</returns>
        public ILog Warn(string message)
        {
            _log.Warning($"({_invocationId}): {message}");
            _log.Flush();
            return this;
        }

        /// <summary>
        /// Fatals the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>ILog.</returns>
        public ILog Fatal(string message)
        {
            _log.Warning($"({_invocationId}): {message}");
            _log.Flush();
            return this;
        }
    }
}

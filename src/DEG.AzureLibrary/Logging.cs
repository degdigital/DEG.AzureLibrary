using DEG.AzureLibrary.Repositories;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Collections.Generic;

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

    /// <summary>
    /// Interface IAppendBlob
    /// </summary>
    public interface IAppendBlob
    {
        /// <summary>
        /// Appends the text.
        /// </summary>
        /// <param name="text">The text.</param>
        void AppendText(string text);
        /// <summary>
        /// Lists the append blobs.
        /// </summary>
        /// <returns>IEnumerable&lt;IListBlobItem&gt;.</returns>
        IEnumerable<IListBlobItem> ListAppendBlobs();
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

    /// <summary>
    /// Class AppendLogger.
    /// </summary>
    /// <seealso cref="DEG.AzureLibrary.Repositories.BaseBlobRepository" />
    /// <seealso cref="DEG.AzureLibrary.IAppendBlob" />
    public class AppendLogger : BaseBlobRepository, IAppendBlob
    {
        readonly string _containerName;
        readonly string _path;
        readonly CloudAppendBlob _append;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppendLogger"/> class.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="path">The path.</param>
        public AppendLogger(string containerName, string path)
            : base(null)
        {
            _containerName = containerName;
            _path = path;
            _append = GetAppendBlob(containerName, path).Result;
        }

        /// <summary>
        /// Appends the text.
        /// </summary>
        /// <param name="text">The text.</param>
        public void AppendText(string text)
        {
            _append.AppendText(text);
        }

        /// <summary>
        /// Lists the append blobs.
        /// </summary>
        /// <returns>IEnumerable&lt;IListBlobItem&gt;.</returns>
        public IEnumerable<IListBlobItem> ListAppendBlobs()
        {
            return ListBlobs(_containerName, _path);
        }
    }
}

using Microsoft.Azure;
using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DEG.AzureLibrary
{
    /// <summary>
    /// Class IssueManager.
    /// </summary>
    public static class IssueManager
    {
        /// <summary>
        /// Processes the issues.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="ctx">The CTX.</param>
        /// <param name="partitionKey">The partition key.</param>
        /// <param name="rowKey">The row key.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="issueRepository">The issue repository.</param>
        /// <exception cref="AggregateException"></exception>
        public static async Task ProcessIssues(this IIssueable source, ExecutionContext ctx = null, string partitionKey = null, string rowKey = null, object tag = null, IIssueRepository issueRepository = null)
        {
            var issues = source.Issues.Where(x => x != null).ToList();
            if (issues.Count == 0)
                return;
            // exit if being throttled
            if ((issueRepository.Flags & IssueRepositoryFlags.ThrottleEmail) == IssueRepositoryFlags.ThrottleEmail)
            {
                int.TryParse(CloudConfigurationManager.GetSetting("NotificationThresholdMinutes") ?? "10", out var throttleInMinutes);
                var emailedOn = issueRepository.GetEmailedOnAsync(partitionKey, rowKey).Result;
                var sinceEmailedOn = DateTime.UtcNow - emailedOn;
                if (sinceEmailedOn < TimeSpan.FromMinutes(throttleInMinutes))
                    return;
            }
            // build message - SendGrid HTML conversion spacing
            // https://sendgrid.com/docs/Classroom/Build/Format_Content/plain_text_emails_converted_to_html.html
            var message = new StringBuilder();
            message.Append($"The following issues were found during the {ctx?.FunctionName}\n\n");
            message.Append($"Log ID: {ctx?.InvocationId}\n\n");
            message.Append(string.Join("\n\n", issues.Select(x => x.ToString(tag)).ToArray()));
            // log history
            var log = source.Log;
            message.Append($"\n\nHistory\n\n {log.History().Replace("\n","\n ")}");
            if ((issueRepository.Flags & IssueRepositoryFlags.Log) == IssueRepositoryFlags.Log)
                log.Info(message.ToString());
            // email message
            if ((issueRepository.Flags & IssueRepositoryFlags.Email) == IssueRepositoryFlags.Email)
            {
                var defaultRecipients = CloudConfigurationManager.GetSetting("EmailNotifyRecipients")?.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                var emailClient = new EmailClient(null);
                var emailResult = emailClient.SendEmail(message.ToString(), issueRepository.Recipients ?? defaultRecipients);
                if ((issueRepository.Flags & IssueRepositoryFlags.ThrottleEmail) == IssueRepositoryFlags.ThrottleEmail)
                    await issueRepository.UpdateEmailedOnAsync(partitionKey, rowKey, DateTime.UtcNow);
                if (emailResult.StatusCode != HttpStatusCode.Accepted) log.Warn($"IssueManager: Failed to Send Email Notification(s)");
                else log.Info($"IssueManager: Emailed Notification(s)");
            }
            // throw if critical
            var criticalIssues = issues.OfType<ICriticalIssue>().Select(x => x.Exception).ToArray();
            if (criticalIssues.Length > 0)
                throw new AggregateException(criticalIssues);
        }
    }

    /// <summary>
    /// Interface IIssue
    /// </summary>
    public interface IIssue
    {
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        string ToString(object x);
    }

    /// <summary>
    /// Interface IIssueable
    /// </summary>
    public interface IIssueable
    {
        /// <summary>
        /// Gets the log.
        /// </summary>
        /// <value>The log.</value>
        ILog Log { get; }
        /// <summary>
        /// Gets the append BLOB.
        /// </summary>
        /// <value>The append BLOB.</value>
        IAppendBlob AppendBlob { get; }
        /// <summary>
        /// Adds the issue.
        /// </summary>
        /// <param name="e">The e.</param>
        void AddIssue(Exception e);
        /// <summary>
        /// Adds the issue.
        /// </summary>
        /// <param name="issues">The issues.</param>
        void AddIssue(params IIssue[] issues);
        /// <summary>
        /// Gets the issues.
        /// </summary>
        /// <value>The issues.</value>
        IList<IIssue> Issues { get; }
    }

    /// <summary>
    /// Interface ICriticalIssue
    /// </summary>
    /// <seealso cref="DEG.AzureLibrary.IIssue" />
    public interface ICriticalIssue : IIssue
    {
        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        /// <value>The exception.</value>
        Exception Exception { get; set; }
    }

    /// <summary>
    /// Interface IIssueRepository
    /// </summary>
    public interface IIssueRepository
    {
        /// <summary>
        /// Gets the flags.
        /// </summary>
        /// <value>The flags.</value>
        IssueRepositoryFlags Flags { get; }
        /// <summary>
        /// Gets the recipients.
        /// </summary>
        /// <value>The recipients.</value>
        string[] Recipients { get; }
        /// <summary>
        /// Gets the emailed on asynchronous.
        /// </summary>
        /// <param name="partitionKey">The partition key.</param>
        /// <param name="rowKey">The row key.</param>
        /// <returns>Task&lt;DateTime&gt;.</returns>
        Task<DateTime> GetEmailedOnAsync(string partitionKey, string rowKey);
        /// <summary>
        /// Updates the emailed on asynchronous.
        /// </summary>
        /// <param name="partitionKey">The partition key.</param>
        /// <param name="rowKey">The row key.</param>
        /// <param name="date">The date.</param>
        /// <returns>Task.</returns>
        Task UpdateEmailedOnAsync(string partitionKey, string rowKey, DateTime date);
    }

    /// <summary>
    /// Enum IssueRepositoryFlags
    /// </summary>
    [Flags]
    public enum IssueRepositoryFlags
    {
        /// <summary>
        /// The log
        /// </summary>
        Log = 1,
        /// <summary>
        /// The email
        /// </summary>
        Email = 2,
        /// <summary>
        /// The throttle email
        /// </summary>
        ThrottleEmail = 4 & Email,
    }

    /// <summary>
    /// Class GeneralIssue.
    /// </summary>
    /// <seealso cref="DEG.AzureLibrary.IIssue" />
    public class GeneralIssue : IIssue
    {
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public string ToString(object x)
        {
            return Message;
        }
    }

    /// <summary>
    /// Class CriticalIssue.
    /// </summary>
    /// <seealso cref="DEG.AzureLibrary.ICriticalIssue" />
    public class CriticalIssue : ICriticalIssue
    {
        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        /// <value>The exception.</value>
        public Exception Exception { get; set; }
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message
        {
            get { return Exception?.Message; }
            set { Exception = new Exception(value); }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public string ToString(object x)
        {
            return Message;
        }
    }
}

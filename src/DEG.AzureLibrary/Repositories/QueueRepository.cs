using Microsoft.Azure;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Queue.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DEG.AzureLibrary.Repositories
{
    /// <summary>
    /// Class QueueRepository.
    /// </summary>
    /// <seealso cref="DEG.AzureLibrary.GlobalSettings" />
    public class QueueRepository : GlobalSettings
    {
        /// <summary>
        /// The issueable
        /// </summary>
        protected readonly IIssueable _issueable;
        /// <summary>
        /// The binder
        /// </summary>
        protected readonly Binder _binder;
        readonly CloudStorageAccount _storageAccount;
        readonly CloudQueueClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueRepository"/> class.
        /// </summary>
        /// <param name="binder">The binder.</param>
        public QueueRepository(Binder binder)
            : this(null, binder) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="QueueRepository"/> class.
        /// </summary>
        /// <param name="issueable">The issueable.</param>
        /// <param name="binder">The binder.</param>
        public QueueRepository(IIssueable issueable, Binder binder)
        {
            _issueable = issueable;
            _binder = binder;
            _storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting(StorageAccountName ?? "AzureWebJobsStorage"));
            _client = _storageAccount.CreateCloudQueueClient();
        }

        /// <summary>
        /// Lists the queues.
        /// </summary>
        /// <param name="prefix">The prefix filter.</param>
        /// <param name="queueListingDetails">The queue listing details.</param>
        /// <param name="options">The options.</param>
        /// <param name="skipPoison">if set to <c>true</c> [skip poison].</param>
        /// <returns>IEnumerable&lt;CloudQueue&gt;.</returns>
        public IEnumerable<CloudQueue> ListQueues(string prefix = null, QueueListingDetails queueListingDetails = QueueListingDetails.All, QueueRequestOptions options = null, bool skipPoison = true)
        {
            var r = _client.ListQueues(prefix, queueListingDetails, options);
            return skipPoison ? r.Where(x => !x.Name.EndsWith("-poison")) : r;
        }

        /// <summary>
        /// Deletes all queued messages.
        /// </summary>
        /// <param name="messagePredicate">The message predicate.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="queueListingDetails">The queue listing details.</param>
        /// <param name="options">The options.</param>
        /// <param name="skipPoison">if set to <c>true</c> [skip poison].</param>
        public void DeleteAllQueuedMessages(Func<CloudQueueMessage, bool> messagePredicate, string prefix = null, QueueListingDetails queueListingDetails = QueueListingDetails.All, QueueRequestOptions options = null, bool skipPoison = true)
        {
            foreach (var queue in ListQueues(prefix, queueListingDetails, options, skipPoison))
                DeleteQueuedMessages(queue, messagePredicate);
        }

        /// <summary>
        /// Deletes the queued messages.
        /// </summary>
        /// <param name="queue">The queue.</param>
        /// <param name="messagePredicate">The message predicate.</param>
        public void DeleteQueuedMessages(CloudQueue queue, Func<CloudQueueMessage, bool> messagePredicate)
        {
            foreach (var message in queue.GetMessages(32))
            {
                if (messagePredicate(message)) queue.DeleteMessage(message);
                else queue.UpdateMessage(message, TimeSpan.Zero, MessageUpdateFields.Visibility);
            }
        }
    }
}

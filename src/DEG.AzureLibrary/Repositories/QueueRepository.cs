using Microsoft.Azure;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Queue.Protocol;
using System.Collections.Generic;

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
        /// Gets the queues.
        /// </summary>
        /// <param name="prefixFilter">The prefix filter.</param>
        /// <param name="queueListingDetails">The queue listing details.</param>
        /// <param name="options">The options.</param>
        /// <returns>IEnumerable&lt;CloudQueue&gt;.</returns>
        public IEnumerable<CloudQueue> GetQueues(string prefixFilter = null, QueueListingDetails queueListingDetails = QueueListingDetails.All, QueueRequestOptions options = null)
        {
            return _client.ListQueues(prefixFilter, queueListingDetails, options);
        }
    }
}

using DEG.AzureLibrary.Exceptions;
using Microsoft.Azure;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DEG.AzureLibrary.Repositories
{
    /// <summary>
    /// Class BlobRepository.
    /// </summary>
    /// <seealso cref="DEG.AzureLibrary.GlobalSettings" />
    public class BlobRepository : GlobalSettings
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
        readonly CloudBlobClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobRepository"/> class.
        /// </summary>
        /// <param name="binder">The binder.</param>
        public BlobRepository(Binder binder)
            : this(null, binder) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="BlobRepository"/> class.
        /// </summary>
        /// <param name="issueable">The issueable.</param>
        /// <param name="binder">The binder.</param>
        public BlobRepository(IIssueable issueable, Binder binder)
        {
            _issueable = issueable;
            _binder = binder;
            _storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting(StorageAccountName ?? "AzureWebJobsStorage"));
            _client = _storageAccount.CreateCloudBlobClient();
        }

        /// <summary>
        /// parse BLOB as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">The path.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        /// <exception cref="BlobParseException"></exception>
        protected async Task<T> ParseBlobAsync<T>(string path)
        {
            try
            {
                using (var reader = await _binder.BindAsync<TextReader>(new[] { new BlobAttribute(path) }))
                    return JsonConvert.DeserializeObject<T>(reader.ReadToEnd(), JsonSerializerSettings);
            }
            catch (Exception e) { throw new BlobParseException(e.ToString()); }
        }

        /// <summary>
        /// Gets the blobs.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="prefixFilter">The prefix filter.</param>
        /// <param name="flatListing">if set to <c>true</c> [flat listing].</param>
        /// <returns>IEnumerable&lt;IListBlobItem&gt;.</returns>
        protected IEnumerable<IListBlobItem> GetBlobs(string containerName, string prefixFilter = null, bool flatListing = false)
        {
            var container = _client.GetContainerReference(containerName);
            return container.ListBlobs(prefixFilter, flatListing);
        }
    }
}

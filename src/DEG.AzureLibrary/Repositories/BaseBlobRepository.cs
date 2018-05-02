using DEG.AzureLibrary.Exceptions;
using Microsoft.Azure;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DEG.AzureLibrary.Repositories
{
    /// <summary>
    /// Class BlobRepository.
    /// </summary>
    /// <seealso cref="DEG.AzureLibrary.GlobalSettings" />
    public abstract class BaseBlobRepository : GlobalSettings
    {
        /// <summary>
        /// The binder
        /// </summary>
        protected readonly Binder _binder;
        readonly CloudStorageAccount _storageAccount;
        readonly CloudBlobClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseBlobRepository"/> class.
        /// </summary>
        /// <param name="binder">The binder.</param>
        public BaseBlobRepository(Binder binder)
        {
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
            if (_binder == null)
                throw new InvalidOperationException("_binder is null");
            try
            {
                using (var r = await _binder.BindAsync<TextReader>(new[] { new BlobAttribute(path) }))
                    return r != null ? JsonConvert.DeserializeObject<T>(r.ReadToEnd(), JsonSerializerSettings) : default(T);
            }
            catch (Exception e) { throw new BlobParseException(e.ToString()); }
        }

        /// <summary>
        /// write BLOB as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">The path.</param>
        /// <param name="obj">The object.</param>
        /// <returns>Task.</returns>
        /// <exception cref="InvalidOperationException">_binder is null</exception>
        /// <exception cref="BlobParseException"></exception>
        protected async Task WriteBlobAsync<T>(string path, T obj)
        {
            if (_binder == null)
                throw new InvalidOperationException("_binder is null");
            try
            {
                using (var r = await _binder.BindAsync<TextWriter>(new[] { new BlobAttribute(path) }))
                    await r.WriteAsync(JsonConvert.SerializeObject(r, JsonSerializerSettings));
            }
            catch (Exception e) { throw new BlobParseException(e.ToString()); }
        }

        /// <summary>
        /// Lists the blobs.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="flatListing">if set to <c>true</c> [flat listing].</param>
        /// <returns>IEnumerable&lt;IListBlobItem&gt;.</returns>
        protected IEnumerable<IListBlobItem> ListBlobs(string containerName, string prefix = null, bool flatListing = false)
        {
            var container = _client.GetContainerReference(containerName);
            if (container == null)
                return Enumerable.Empty<IListBlobItem>();
            return container.ListBlobs(prefix, flatListing).ToList();
        }

        /// <summary>
        /// Removes the BLOB.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="path">The path.</param>
        protected void RemoveBlob(string containerName, string path)
        {
            var container = _client.GetContainerReference(containerName);
            if (container == null)
                return;
            var blob = container.GetAppendBlobReference(path);
            blob.DeleteIfExists();
        }

        /// <summary>
        /// Gets the append BLOB.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="path">The path.</param>
        /// <returns>CloudAppendBlob.</returns>
        protected async Task<CloudAppendBlob> GetAppendBlob(string containerName, string path)
        {
            var container = _client.GetContainerReference(containerName);
            if (container == null)
                return null;
            await container.CreateIfNotExistsAsync();
            var blob = container.GetAppendBlobReference(path);
            await blob.CreateOrReplaceAsync();
            return blob;
        }
    }
}

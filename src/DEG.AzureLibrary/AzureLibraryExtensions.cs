using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ExecutionContext = Microsoft.Azure.WebJobs.ExecutionContext;

namespace DEG.AzureLibrary
{
    /// <summary>
    /// Class AzureFunctionExtensions.
    /// </summary>
    public static class AzureLibraryExtensions
    {
        /// <summary>
        /// execute query as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table">The table.</param>
        /// <param name="query">The query.</param>
        /// <param name="ct">The ct.</param>
        /// <param name="onProgress">The on progress.</param>
        /// <param name="onIssue">The on issue.</param>
        /// <returns>Task&lt;IList&lt;T&gt;&gt;.</returns>
        public static async Task<IList<T>> ExecuteQueryAsync<T>(this CloudTable table, TableQuery<T> query, CancellationToken ct = default(CancellationToken), Action<IList<T>> onProgress = null, IIssueable onIssue = null)
            where T : ITableEntity, new()
        {
            var items = new List<T>();
            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<T> seg = null;
                try { seg = await table.ExecuteQuerySegmentedAsync(query, token); }
                catch (Exception e)
                {
                    WebException we;
                    if ((we = e.InnerException as WebException) == null || await HandleExecuteQueryException(we, ct, "ExecuteQuery", onIssue))
                    {
                        onIssue?.AddIssue(null, new ExecuteIssue { Command = "ExecuteQuery", Message = e.Message });
                        throw;
                    }
                    continue;
                }
                token = seg.ContinuationToken;
                items.AddRange(seg);
                onProgress?.Invoke(items);
            } while (token != null && !ct.IsCancellationRequested);
            return items;
        }

        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table">The table.</param>
        /// <param name="fiber">The fiber.</param>
        /// <param name="query">The query.</param>
        /// <param name="ct">The ct.</param>
        /// <param name="onProgress">The on progress.</param>
        /// <param name="onIssue">The on issue.</param>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        public static IEnumerable<T> ExecuteQuery<T>(this CloudTable table, GenericFiber fiber, TableQuery<T> query, CancellationToken ct = default(CancellationToken), Action onProgress = null, IIssueable onIssue = null)
            where T : ITableEntity, new()
        {
            do
            {
                TableQuerySegment<T> seg = null;
                try { seg = table.ExecuteQuerySegmentedAsync(query, (TableContinuationToken)fiber.Cursor).Result; }
                catch (Exception e)
                {
                    WebException we;
                    if ((we = e.InnerException as WebException) == null || HandleExecuteQueryException(we, ct, "ExecuteQueryFiber", onIssue).Result)
                    {
                        onIssue?.AddIssue(null, new ExecuteIssue { Command = "ExecuteQueryFiber", Message = e.Message });
                        throw;
                    }
                    continue;
                }
                foreach (var x in seg.Results.Skip(fiber.CursorAt))
                {
                    fiber.PendingAt++;
                    yield return x;
                    onProgress?.Invoke();
                }
                // advance
                fiber.Cursor = seg.ContinuationToken;
                fiber.CursorAt = 0;
            } while (fiber.Cursor != null && !ct.IsCancellationRequested);
        }

        /// <summary>
        /// execute as an asynchronous operation.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="operation">The operation.</param>
        /// <param name="onIssue">The on issue.</param>
        /// <returns>Task&lt;TableResult&gt;.</returns>
        public static async Task<TableResult> ExecuteAsync(this CloudTable table, TableOperation operation, IIssueable onIssue = null)
        {
            try { return await table.ExecuteAsync(operation); }
            catch (Exception e)
            {
                WebException we;
                if ((we = e.InnerException as WebException) == null || HandleExecuteQueryException(we, default(CancellationToken), "Execute", onIssue).Result)
                {
                    onIssue?.AddIssue(null, new ExecuteIssue { Command = "Execute", Message = e.Message });
                    throw;
                }
                throw;
            }
        }

        /// <summary>
        /// execute batch as an asynchronous operation.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="batch">The batch.</param>
        /// <param name="onIssue">The on issue.</param>
        /// <returns>Task&lt;IList&lt;TableResult&gt;&gt;.</returns>
        public static async Task<IList<TableResult>> ExecuteBatchAsync(this CloudTable table, TableBatchOperation batch, IIssueable onIssue = null)
        {
            //try { return await table.ExecuteBatchAsync(batch); }
            try
            {
                foreach (var operation in batch)
                {
                    await table.ExecuteAsync(operation);
                }
                return null;
            }
            catch (Exception e)
            {
                WebException we;
                if ((we = e.InnerException as WebException) == null || HandleExecuteQueryException(we, default(CancellationToken), "Execute", onIssue).Result)
                {
                    onIssue?.AddIssue(null, new ExecuteIssue { Command = "Execute", Message = e.Message });
                    throw;
                }
                throw;
            }
        }

        private static async Task<bool> HandleExecuteQueryException(WebException we, CancellationToken ct, string command, IIssueable onIssue)
        {
            var wr = (HttpWebResponse)we.Response;
            if ((int)wr.StatusCode != 429)
                return true;
            onIssue?.AddIssue(null, new ExecuteThrottleIssue { Command = command });
            var sleepTime = new TimeSpan(500);
            await Task.Delay(sleepTime, ct);
            return false;
        }

        /// <summary>
        /// Compresses the string.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] CompressString(string text) { return Compress(Encoding.UTF8.GetBytes(text)); }
        /// <summary>
        /// Decompresses the string.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>System.String.</returns>
        public static string DecompressString(byte[] text) { var data = Decompress(text); return data != null ? Encoding.UTF8.GetString(data) : string.Empty; }

        /// <summary>
        /// Compresses the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] Compress(byte[] data)
        {
            using (var ms = new MemoryStream())
            {
                var bw = new BinaryWriter(ms);
                bw.Write(data.Length);
                using (var ds = new GZipStream(ms, CompressionMode.Compress))
                    ds.Write(data, 0, data.Length);
                var compressed = ms.ToArray();
                return compressed;
            }
        }

        /// <summary>
        /// Decompresses the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] Decompress(byte[] data)
        {
            if (data.Length == 0)
                return null;
            try
            {
                using (var ms = new MemoryStream(data))
                {
                    var br = new BinaryReader(ms);
                    var buffer = new byte[br.ReadInt32()];
                    using (var zip = new GZipStream(ms, CompressionMode.Decompress))
                        zip.Read(buffer, 0, buffer.Length);
                    return buffer;
                }
            }
            catch { return null; }
        }

        #region Issues

        private class ExecuteThrottleIssue : IIssue
        {
            public string Command { get; set; }

            public string ToString(object x) { return $"{Command} ExecuteThrottle"; }
        }

        private class ExecuteIssue : IIssue
        {
            public string Command { get; set; }
            public string Message { get; set; }

            public string ToString(object x) { return $"{Command} Execute: {Message}"; }
        }

        #endregion
    }
}
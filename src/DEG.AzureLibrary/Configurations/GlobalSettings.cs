using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace DEG.AzureLibrary
{
    /// <summary>
    /// Class GlobalSettings.
    /// </summary>
    public class GlobalSettings
    {
        /// <summary>
        /// Gets the application identifier.
        /// </summary>
        /// <value>The application identifier.</value>
        public static string AppId { get; set; }
        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        public static string ConnectionString { get; set; }
        /// <summary>
        /// Gets the name of the storage account.
        /// </summary>
        /// <value>The name of the storage account.</value>
        public static string StorageAccountName { get; set; }
        /// <summary>
        /// Gets the default from email.
        /// </summary>
        /// <value>The default from email.</value>
        public static string DefaultFromEmail { get; set; }
        /// <summary>
        /// Gets the default from name.
        /// </summary>
        /// <value>The default from name.</value>
        public static string DefaultFromName { get; set; }

        static GlobalSettings()
        {
            AppId = "APP";
        }

        /// <summary>
        /// The json serializer settings
        /// </summary>
        public static JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            MissingMemberHandling = MissingMemberHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Formatting = Formatting.Indented,
            ContractResolver = new CamelCasePropertyNamesContractResolver { NamingStrategy = new CamelCaseNamingStrategy() }
        };

        /// <summary>
        /// Gets the table request options.
        /// </summary>
        /// <value>The table request options.</value>
        public static TableRequestOptions TableRequestOptions => new TableRequestOptions
        {
            RetryPolicy = new ExponentialRetry(TimeSpan.FromMilliseconds(2), 100)
        };
    }
}

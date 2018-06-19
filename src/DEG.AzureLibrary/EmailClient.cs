using Microsoft.Azure;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace DEG.AzureLibrary
{
    /// <summary>
    /// Interface IEmailClient
    /// </summary>
    public interface IEmailClient
    {
        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="recipients">The recipients.</param>
        /// <param name="from">From.</param>
        /// <param name="subject">The subject.</param>
        /// <returns>Response.</returns>
        Response SendEmail(string message, string[] recipients, EmailAddress from = null, string subject = null);
    }

    /// <summary>
    /// Class EmailClient.
    /// </summary>
    /// <seealso cref="DEG.AzureLibrary.GlobalSettings" />
    /// <seealso cref="DEG.AzureLibrary.IEmailClient" />
    public class EmailClient : GlobalSettings, IEmailClient
    {
        /// <summary>
        /// The API key
        /// </summary>
        protected readonly string _apiKey;
        /// <summary>
        /// The client
        /// </summary>
        protected readonly SendGridClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailClient"/> class.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        public EmailClient(string apiKey = null)
        {
            _apiKey = apiKey ?? CloudConfigurationManager.GetSetting("SENDGRID_API_KEY");
            _client = new SendGridClient(_apiKey);
        }

        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="recipients">The recipients.</param>
        /// <param name="from">From.</param>
        /// <param name="subject">The subject.</param>
        /// <returns>Response.</returns>
        public Response SendEmail(string message, string[] recipients, EmailAddress from = null, string subject = null)
        {
            var msg = new SendGridMessage
            {
                From = from ?? new EmailAddress(DefaultFromEmail, DefaultFromName),
                Subject = subject ?? $"{AppId} Notification",
                PlainTextContent = message
                //HtmlContent = message.Replace(@"\n", "<br>")
            };
            foreach (var recipient in recipients)
                msg.AddTo(recipient, null);
            return _client.SendEmailAsync(msg).Result;
        }
    }
}

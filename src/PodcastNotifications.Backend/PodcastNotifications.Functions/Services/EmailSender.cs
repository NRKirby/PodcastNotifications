using System;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace PodcastNotifications.Functions.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly ISendGridClient _sendGridClient;

        public EmailSender(ISendGridClient sendGridClient)
        {
            _sendGridClient = sendGridClient;
        }

        public async Task<Response> Send(string subject, string htmlContent, string toEmailAddress)
        {
            var from = new EmailAddress("no_reply@podcastnotifications", "Podcast Notifications");
            var to = new EmailAddress(toEmailAddress);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, htmlContent, htmlContent);
            var response = await _sendGridClient.SendEmailAsync(msg).ConfigureAwait(false);
            return response;
        }
    }
}

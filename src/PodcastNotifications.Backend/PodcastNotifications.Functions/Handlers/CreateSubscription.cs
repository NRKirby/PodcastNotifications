using System;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using PodcastNotifications.Functions.Entities;
using PodcastNotifications.Functions.Infrastructure;
using PodcastNotifications.Functions.Repositories;
using PodcastNotifications.Functions.Services;

namespace PodcastNotifications.Functions.Handlers
{
    public class CreateSubscription
    {
        public class Command : IRequest<CommandResult>
        {
            public string SubscriptionId { get; set; }
            public string EmailAddress { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, CommandResult>
        {
            private readonly IEmailSender _emailSender;
            private readonly ITableRepository<PodcastSubscription> _repository;
            private readonly IFeedContentStorageApi _feedContentStorage;
            private readonly IConfiguration _configuration;

            public CommandHandler(IEmailSender emailSender, 
                ITableRepository<PodcastSubscription> repository, 
                IFeedContentStorageApi feedContentStorage, 
                IConfiguration configuration)
            {
                _emailSender = emailSender;
                _repository = repository;
                _feedContentStorage = feedContentStorage;
                _configuration = configuration;
            }

            public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var normalizedEmail = request.EmailAddress?.Trim().ToLowerInvariant();
                if (!IsValidEmail(normalizedEmail))
                {
                    return CommandResult.BadRequest($@"The email address ""{normalizedEmail}"" is invalid.");
                }

                var subscriptionToken = Guid.NewGuid().ToString();
                var feedTitle = await _feedContentStorage.GetFeedTitle(request.SubscriptionId);
                var html = GetConfirmationHtml(subscriptionToken, feedTitle);

                var response = await _emailSender.Send($@"Please confirm your subscription to {GetPodcastName(feedTitle)}", html, normalizedEmail);
                if (!response.IsSuccessStatusCode)
                {
                    return CommandResult.BadRequest($"There was an error: {response.StatusCode}");
                }

                var podcastSubscription = new PodcastSubscription(SubscriptionState.Pending,
                    subscriptionToken, request.SubscriptionId, normalizedEmail, feedTitle);

                await _repository.InsertAsync(podcastSubscription);
                return CommandResult.Ok();
            }

            private static bool IsValidEmail(string email)
            {
                try
                {
                    var addr = new MailAddress(email);
                    return addr.Address == email;
                }
                catch
                {
                    return false;
                }
            }

            private static string GetPodcastName(string title) 
                => title.ToLowerInvariant().Contains("podcast") ? title : $"{title} Podcast";

            private string GetConfirmationHtml(string subscriptionToken, string feedTitle)
            {
                return $@"
<h1>Podcast Notifications</h1>
<p>Please confirm your subscription to {GetPodcastName(feedTitle)} by clicking the following link</p>
{_configuration["FrontendBaseUrl"]}subscription/confirm/{subscriptionToken}";
            }
        }
    }
}

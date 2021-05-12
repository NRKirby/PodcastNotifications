using System;
using System.Collections.Generic;
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
    public class ConfirmSubscription
    {
        public class Command : IRequest<CommandResult>
        {
            public string SubscriptionToken { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, CommandResult>
        {
            private readonly ITableRepository<PodcastSubscription> _repository;
            private readonly IFeedContentStorageApi _feedContentClient;
            private readonly IEmailSender _emailSender;
            private readonly IConfiguration _configuration;

            public CommandHandler(ITableRepository<PodcastSubscription> repository,
                IFeedContentStorageApi feedContentClient, IEmailSender emailSender, IConfiguration configuration)
            {
                _repository = repository;
                _feedContentClient = feedContentClient;
                _emailSender = emailSender;
                _configuration = configuration;
            }

            public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var podcastSubscription = await _repository.RetrieveAsync(SubscriptionState.Pending.ToString(), request.SubscriptionToken);
                if (podcastSubscription == null)
                {
                    return CommandResult.BadRequest("The subscription token was not valid.");
                }

                try
                {
                    var unsubscribeLink = $"{_configuration["FrontendBaseUrl"]}subscription/unsubscribe/{request.SubscriptionToken}";

                    await _feedContentClient.CreateFeedItemPublishedSubscription(podcastSubscription.FeedRowKey,
                        podcastSubscription.EmailAddress, unsubscribeLink);

                    var confirmedSubscription = PodcastSubscription.CreateConfirmed(podcastSubscription);
                    await _repository.InsertAsync(confirmedSubscription);
                    await _repository.RemoveAsync(podcastSubscription);

                    await _emailSender.Send($"{podcastSubscription.FeedTitle} subscription confirmed",
                        GetHtml(podcastSubscription.FeedTitle, unsubscribeLink), podcastSubscription.EmailAddress);

                    return CommandResult.Ok();
                }
                catch (Exception ex)
                {
                    // TODO: log exception in a table as App Insights is expensive
                    return CommandResult.InternalServerError(new List<string>
                    {
                        $"An error occurred creating the subscription for {podcastSubscription.FeedRowKey} due to {ex}."
                    });
                }
            }

            private static string GetHtml(string feedTitle, string unsubscribeLink)
                => $@"
<h1>Subscription confirmed</h1>
<p>You will now receive an email each time a {feedTitle} episode gets released.</p>
<p>You can <a href=""{unsubscribeLink}"">unsubscribe</a> at any time</p>";
        }
    }
}

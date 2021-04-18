using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PodcastNotifications.Functions.Entities;
using PodcastNotifications.Functions.Infrastructure;
using PodcastNotifications.Functions.Repositories;
using PodcastNotifications.Functions.Services;

namespace PodcastNotifications.Functions.Handlers
{
    public class CancelSubscription
    {
        public class Command : IRequest<CommandResult>
        {
            public string SubscriptionToken { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, CommandResult>
        {
            private readonly ITableRepository<PodcastSubscription> _repository;
            private readonly IFeedContentStorageApi _feedContentClient;

            public CommandHandler(ITableRepository<PodcastSubscription> repository,
                IFeedContentStorageApi feedContentClient)
            {
                _repository = repository;
                _feedContentClient = feedContentClient;
            }

            public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var confirmedSubscription = await _repository.RetrieveAsync(SubscriptionState.Confirmed.ToString(), request.SubscriptionToken);
                if (confirmedSubscription == null)
                {
                    return CommandResult.BadRequest("The token provided was not valid.");
                }

                try
                {
                    await _feedContentClient.RemoveFeedItemPublishedSubscription(confirmedSubscription.FeedRowKey,
                        confirmedSubscription.EmailAddress);

                    var cancelledSubscription = PodcastSubscription.CreateCancelled(confirmedSubscription);
                    await _repository.InsertAsync(cancelledSubscription);
                    await _repository.RemoveAsync(confirmedSubscription);

                    return CommandResult.Ok();
                }
                catch (Exception ex)
                {
                    // TODO: log exception in a table as App Insights is expensive
                    return CommandResult.InternalServerError(new List<string>
                    {
                        $"An error occurred creating the subscription for {confirmedSubscription.FeedRowKey} due to {ex}."
                    });
                }
            }
        }
    }
}

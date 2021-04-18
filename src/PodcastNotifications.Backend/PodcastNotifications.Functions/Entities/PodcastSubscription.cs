using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace PodcastNotifications.Functions.Entities
{
    public class PodcastSubscription : TableEntity
    {
        public PodcastSubscription() { }

        public PodcastSubscription(SubscriptionState subscriptionState, string subscriptionToken, string feedRowKey, string emailAddress, string feedTitle)
        {
            if (subscriptionState == SubscriptionState.NotSet) throw new InvalidOperationException("Subscription state has not been set.");

            PartitionKey = subscriptionState.ToString();
            RowKey = subscriptionToken;
            FeedRowKey = feedRowKey;
            EmailAddress = emailAddress;
            FeedTitle = feedTitle;
        }

        public string FeedRowKey { get; set; }
        public string FeedTitle { get; set; }
        public string EmailAddress { get; set; }

        public static PodcastSubscription CreateConfirmed(PodcastSubscription podcastSubscription)
            => new PodcastSubscription(SubscriptionState.Confirmed,
                podcastSubscription.RowKey,
                podcastSubscription.FeedRowKey,
                podcastSubscription.EmailAddress, 
                podcastSubscription.FeedTitle);

        public static PodcastSubscription CreateCancelled(PodcastSubscription podcastSubscription)
            => new PodcastSubscription(SubscriptionState.Cancelled,
                podcastSubscription.RowKey,
                podcastSubscription.FeedRowKey,
                podcastSubscription.EmailAddress,
                podcastSubscription.FeedTitle);

    }

    public enum SubscriptionState
    {
        NotSet = 0,
        Pending = 1,
        Confirmed = 2,
        Cancelled,
    }
}

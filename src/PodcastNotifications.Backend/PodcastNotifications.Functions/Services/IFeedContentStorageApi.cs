using System.Collections.Generic;
using System.Threading.Tasks;
using PodcastNotifications.Functions.Models;

namespace PodcastNotifications.Functions.Services
{
    public interface IFeedContentStorageApi
    {
        Task CreateFeedItemPublishedSubscription(string feedRowKey, string toEmailAddress, string unsubscribeLink);
        Task RemoveFeedItemPublishedSubscription(string feedRowKey, string toEmailAddress);
        Task<IEnumerable<Podcast>> GetAllPodcasts();
        Task<string> GetFeedTitle(string feedRowKey);
    }
}

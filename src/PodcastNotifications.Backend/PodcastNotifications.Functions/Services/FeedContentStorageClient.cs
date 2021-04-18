using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PodcastNotifications.Functions.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace PodcastNotifications.Functions.Services
{
    public class FeedContentStorageClient : IFeedContentStorageApi
    {
        private readonly HttpClient _httpClient;
        private readonly FeedContentStorageSettings _settings;

        public FeedContentStorageClient(HttpClient httpClient, FeedContentStorageSettings settings)
        {
            _httpClient = httpClient;
            _settings = settings;
        }

        public async Task CreateFeedItemPublishedSubscription(string feedRowKey, string toEmailAddress, string unsubscribeLink)
        {
            var model = new
            {
                FeedItemPartitionKey = feedRowKey,
                Type = "Podcast",
                ToEmailAddress = toEmailAddress,
                FromName = _settings.FromName,
                FromEmailAddress = _settings.FromEmailAddress,
                UnsubscribeLink = unsubscribeLink
            };
            var json = JsonSerializer.Serialize(model);

            var response = await _httpClient.PostAsync($"CreateFeedItemPublishedSubscription?code={_settings.HostKey}", 
                new StringContent(json, Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();
        }

        public async Task RemoveFeedItemPublishedSubscription(string feedRowKey, string toEmailAddress)
        {
            var model = new
            {
                FeedItemPartitionKey = feedRowKey,
                Type = "Podcast",
                ToEmailAddress = toEmailAddress,
                FromName = _settings.FromName,
                FromEmailAddress = _settings.FromEmailAddress
            };
            var json = JsonSerializer.Serialize(model);

            var response = await _httpClient.PostAsync($"RemoveFeedItemPublishedSubscription?code={_settings.HostKey}",
                new StringContent(json, Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<Podcast>> GetAllPodcasts()
        {
            var response = await _httpClient.GetAsync("GetFeeds?type=Podcast");
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<IEnumerable<Podcast>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<string> GetFeedTitle(string feedRowKey)
        {
            var response = await _httpClient.GetAsync($"GetFeed?code={_settings.HostKey}&partitionKey=Podcast&rowKey={feedRowKey}");
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Podcast>(json);
            return result.Title;
        }
    }
}

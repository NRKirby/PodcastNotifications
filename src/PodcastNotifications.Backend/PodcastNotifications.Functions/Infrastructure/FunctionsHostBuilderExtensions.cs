using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PodcastNotifications.Functions.Entities;
using PodcastNotifications.Functions.Repositories;
using PodcastNotifications.Functions.Services;
using PodcastNotifications.Functions.Storage;
using SendGrid;

namespace PodcastNotifications.Functions.Infrastructure
{
    public static class FunctionsHostBuilderExtensions
    {
        public static IFunctionsHostBuilder AddServices(this IFunctionsHostBuilder builder, IConfiguration configuration)
        {
            var storageConnection = configuration["AzureStorageConnectionString"];
            var blobContainerName = configuration["BlobContainerName"];
            var settings = new BlobClientSettings
            {
                ConnectionString = storageConnection,
                ContainerName = blobContainerName
            };
            settings.Validate();
            builder.Services.AddTransient(_ => new BlobClient(settings));
            var sendGridApiKey = configuration["SendGridApiKey"];
            builder.Services.AddTransient<IEmailSender, EmailSender>(_ => 
                new EmailSender(new SendGridClient(sendGridApiKey)));

            var feedContentStorageSettings = configuration.GetSection("FeedContentStorageSettings").Get<FeedContentStorageSettings>();
            builder.Services.AddSingleton(feedContentStorageSettings);
            builder.Services.AddHttpClient<IFeedContentStorageApi, FeedContentStorageClient>(httpClient =>
            {
                var baseUrl = configuration["FeedContentStorageBaseUrl"];
                httpClient.BaseAddress = new Uri(baseUrl);
            });

            return builder;
        }

        public static IFunctionsHostBuilder AddRepositories(this IFunctionsHostBuilder builder, IConfiguration configuration)
        {
            var storageConnection = configuration["AzureStorageConnectionString"];
            var podcastSubscriptionTableName = configuration["PodcastSubscriptionTableName"];
            builder.Services.AddTransient<ITableRepository<PodcastSubscription>, PodcastSubscriptionRepository>(_ => 
                new PodcastSubscriptionRepository(storageConnection, podcastSubscriptionTableName));

            return builder;
        }
    }
}

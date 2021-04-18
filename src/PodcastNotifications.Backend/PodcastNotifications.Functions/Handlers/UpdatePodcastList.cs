using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PodcastNotifications.Functions.Infrastructure;
using PodcastNotifications.Functions.Models;
using PodcastNotifications.Functions.Services;
using PodcastNotifications.Functions.Storage;

namespace PodcastNotifications.Functions.Handlers
{
    public class UpdatePodcastList
    {
        public class Command : IRequest<CommandResult> { }

        public class CommandHandler : IRequestHandler<Command, CommandResult>
        {
            private readonly IFeedContentStorageApi _feedContentStorage;
            private readonly BlobClient _blobClient;

            public CommandHandler(BlobClient blobClient, IFeedContentStorageApi feedContentStorage)
            {
                _blobClient = blobClient;
                _feedContentStorage = feedContentStorage;
            }

            public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var podcasts = await _feedContentStorage.GetAllPodcasts();
                var grouped = podcasts.GroupBy(x => char.ToUpper(x.Title[0]));
                var model = new Model
                {
                    Items = grouped.Select(x =>
                        new CharIndex
                        {
                            Index = x.Key.ToString(),
                            Data = x.ToList()
                        })
                };
                var serializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
                var json = JsonConvert.SerializeObject(model, serializerSettings);

                await _blobClient.Upload(json, "index.json");

                return CommandResult.Ok();
            }
        }

        public class Model
        {
            public IEnumerable<CharIndex> Items { get; set; } = new Collection<CharIndex>();
        }
    }
}

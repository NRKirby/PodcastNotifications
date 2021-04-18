using System.Threading.Tasks;
using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using PodcastNotifications.Functions.Handlers;

namespace PodcastNotifications.Functions.Functions
{
    public class UpdatePodcasts
    {
        private readonly IMediator _mediator;

        public UpdatePodcasts(IMediator mediator)
        {
            _mediator = mediator;
        }

        [FunctionName(nameof(UpdatePodcasts))]
        public async Task Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, ILogger log)
        {
            await _mediator.Send(new UpdatePodcastList.Command());
        }
    }
}

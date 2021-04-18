using System.IO;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace PodcastNotifications.Functions.Functions
{
    public class ConfirmSubscription
    {
        private readonly IMediator _mediator;

        public ConfirmSubscription(IMediator mediator)
        {
            _mediator = mediator;
        }

        [FunctionName("confirm-subscription")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var command = JsonConvert.DeserializeObject<Handlers.ConfirmSubscription.Command> (requestBody);
            if (command == null)
            {
                return new BadRequestObjectResult("The request was not valid.");
            }

            return await _mediator.Send(command);
        }
    }
}


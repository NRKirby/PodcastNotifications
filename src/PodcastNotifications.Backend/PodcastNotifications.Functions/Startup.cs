using System.Reflection;
using MediatR;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using PodcastNotifications.Functions;
using PodcastNotifications.Functions.Infrastructure;

[assembly: FunctionsStartup(typeof(Startup))]
namespace PodcastNotifications.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddMediatR(Assembly.GetAssembly(typeof(Startup)));
            var configuration = builder.GetContext().Configuration;

            builder.AddServices(configuration);
            builder.AddRepositories(configuration);
        }
    }
}

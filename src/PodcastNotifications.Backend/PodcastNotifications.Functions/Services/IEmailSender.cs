using System.Threading.Tasks;
using SendGrid;

namespace PodcastNotifications.Functions.Services
{
    public interface IEmailSender
    {
        Task<Response> Send(string subject, string htmlContent, string toEmailAddress);
    }
}

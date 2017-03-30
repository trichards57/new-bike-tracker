using System.Threading.Tasks;

namespace BikeTracker.Core.Services.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}

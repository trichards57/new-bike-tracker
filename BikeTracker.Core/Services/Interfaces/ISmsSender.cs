using System.Threading.Tasks;

namespace BikeTracker.Core.Services.Interfaces
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}

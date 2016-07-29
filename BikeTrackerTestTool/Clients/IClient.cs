using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace BikeTrackerTestTool.Clients
{
    public interface IClient : INotifyPropertyChanged
    {
        Task<string> SendUpdate(Uri path);
        string Imei { get; set; }
        decimal BaseLatitude { get; set; }
        decimal BaseLongitude { get; set; }
        double FailureRate { get; set; }
        string ResponseString { get; }
    }
}

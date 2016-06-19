using System.Threading.Tasks;

namespace BikeTrackerTestTool.Clients
{
    interface IClient
    {
        Task<string> SendUpdate();
        string Imei { get; set; }
        float BaseLatitude { get; set; }
        float BaseLongitude { get; set; }
        double FailureRate { get; set; }
    }
}

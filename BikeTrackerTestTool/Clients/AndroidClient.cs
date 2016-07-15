using GalaSoft.MvvmLight;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace BikeTrackerTestTool.Clients
{
    public class AndroidClient : ViewModelBase, IClient
    {
        private readonly Random random = new Random();

        private decimal baseLatitude;
        private decimal baseLongitude;
        private double failureRate;
        private string imei;
        private string responseString = "[No Response Received]";

        public string Name
        {
            get
            {
                return $"Android Client : {imei}";
            }
        }

        public decimal BaseLatitude
        {
            get
            {
                return baseLatitude;
            }
            set
            {
                Set(ref baseLatitude, value);
            }
        }

        public decimal BaseLongitude
        {
            get
            {
                return baseLongitude;
            }
            set
            {
                Set(ref baseLongitude, value);
            }
        }

        public double FailureRate
        {
            get
            {
                return failureRate;
            }
            set
            {
                Set(ref failureRate, value);
            }
        }

        public string Imei
        {
            get
            {
                return imei;
            }
            set
            {
                Set(ref imei, value);
                RaisePropertyChanged(() => Name);
            }
        }

        public string ResponseString
        {
            get
            {
                return responseString;
            }

            private set
            {
                Set(ref responseString, value);
            }
        }

        public async Task<string> SendUpdate(Uri path)
        {
            var lat = BaseLatitude + (decimal)(random.NextDouble() * 0.0001);
            var lon = BaseLongitude + (decimal)(random.NextDouble() * 0.0001);

            var time = DateTime.Now;

            var tString = time.ToString("o");

            Uri uri;

            if (random.NextDouble() < FailureRate)
            {
                var type = random.Next(4);
                switch (type)
                {
                    case 0:// No Location
                        uri = new Uri($"{path}/Map/CheckIn?imei={Imei}&time={tString}&v=2");
                        break;

                    case 1:// No Date or Time
                        uri = new Uri($"{path}/Map/CheckIn?imei={Imei}&lat={lat}&lon={lon}&v=2");
                        break;

                    case 2:// No IMEI
                        uri = new Uri($"{path}/Map/CheckIn?lat={lat}&lon={lon}&time={tString}&v=2");
                        break;

                    // Bad Date or Time
                    default:
                        uri = new Uri($"{path}/Map/CheckIn?imei={Imei}&lat={lat}&lon={lon}&time=123&v=2");
                        break;
                }
            }
            else
                uri = new Uri($"{path}/Map/CheckIn?lat={lat}&lon={lon}&imei={Imei}&time={tString}&v=2");

            var client = new WebClient();

            try
            {
                ResponseString = await client.DownloadStringTaskAsync(uri);
                return ResponseString;
            }
            catch (WebException e)
            {
                var stream = new StreamReader(e.Response.GetResponseStream());
                ResponseString = await stream.ReadToEndAsync();
                return ResponseString;
            }
        }
    }
}

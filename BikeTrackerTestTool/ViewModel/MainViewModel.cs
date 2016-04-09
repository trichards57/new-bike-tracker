using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;

namespace BikeTrackerTestTool.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly Random random = new Random();
        private double failureRate;
        private string imei;
        private decimal latitude;
        private decimal longitude;
        private string responseString = "[Nothing Received]";
        private ServerLocation selectedLocation;
        private int updateRate;
        private bool updateRunning;
        private Timer updateTimer;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            Locations = new ObservableCollection<ServerLocation>
            {
                new ServerLocation
                {
                     Name = "Local",
                     Path = new Uri("http://localhost:6088")
                },
                new ServerLocation
                {
                    Name = "Production",
                    Path = new Uri("http://sjatracker.elasticbeanstalk.com")
                }
            };
            SelectedLocation = Locations.First();
            UpdateRate = 15;
            IMEI = "1234";
            Latitude = 51.532M;
            Longitude = -2.552M;
            StartUpdate = new RelayCommand(ExecuteStartUpdate, CanExecuteStartUpdate);
            StopUpdate = new RelayCommand(ExecuteStopUpdate, CanExecuteStopUpdate);
            FailureRate = 0.1;
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

        public string IMEI
        {
            get
            {
                return imei;
            }
            set
            {
                Set(ref imei, value);
            }
        }

        public decimal Latitude
        {
            get
            {
                return latitude;
            }
            set
            {
                Set(ref latitude, value);
            }
        }

        public ObservableCollection<ServerLocation> Locations { get; }

        public decimal Longitude
        {
            get
            {
                return longitude;
            }
            set
            {
                Set(ref longitude, value);
            }
        }

        public string ResponseString
        {
            get
            {
                return responseString;
            }
            set
            {
                Set(ref responseString, value);
            }
        }

        public ServerLocation SelectedLocation
        {
            get
            {
                return selectedLocation;
            }
            set
            {
                Set(ref selectedLocation, value);
            }
        }

        public RelayCommand StartUpdate { get; }

        public RelayCommand StopUpdate { get; }

        public int UpdateRate
        {
            get
            {
                return updateRate;
            }
            set
            {
                Set(ref updateRate, value);
            }
        }

        public bool UpdateRunning
        {
            get
            {
                return updateRunning;
            }
            set
            {
                updateRunning = value;
            }
        }

        private bool CanExecuteStartUpdate()
        {
            return !updateRunning;
        }

        private bool CanExecuteStopUpdate()
        {
            return updateRunning;
        }

        private void ExecuteStartUpdate()
        {
            updateTimer = new Timer(UpdateLocation, null, 0, UpdateRate * 1000);
            UpdateRunning = true;
            StartUpdate.RaiseCanExecuteChanged();
            StopUpdate.RaiseCanExecuteChanged();
        }

        private void ExecuteStopUpdate()
        {
            updateTimer.Change(Timeout.Infinite, Timeout.Infinite);
            updateTimer.Dispose();
            updateTimer = null;

            UpdateRunning = false;
            StartUpdate.RaiseCanExecuteChanged();
            StopUpdate.RaiseCanExecuteChanged();
        }

        private async void UpdateLocation(object state)
        {
            var lat = Latitude + (decimal)(random.NextDouble() * 0.0001);
            var lon = Longitude + (decimal)(random.NextDouble() * 0.0001);

            var time = DateTime.Now;

            var tString = time.ToString("HHmmss.fff");
            var dString = time.ToString("ddMMyy");

            Uri uri;

            if (random.NextDouble() < FailureRate)
            {
                var type = random.Next(4);
                switch (type)
                {
                    case 0:// No Location
                        uri = new Uri($"{SelectedLocation.Path.ToString()}/Map/CheckIn?imei={IMEI}&time={tString}&date={dString}");
                        break;

                    case 1:// No Date or Time
                        uri = new Uri($"{SelectedLocation.Path.ToString()}/Map/CheckIn?imei={IMEI}&lat={lat}&lon={lon}");
                        break;

                    case 2:// No IMEI
                        uri = new Uri($"{SelectedLocation.Path.ToString()}/Map/CheckIn?lat={lat}&lon={lon}&time={tString}&date={dString}");
                        break;

                    case 3:// Bad Date or Time
                    default:
                        uri = new Uri($"{SelectedLocation.Path.ToString()}/Map/CheckIn?imei={IMEI}&lat={lat}&lon={lon}&time=123&date=456");
                        break;
                }
            }
            else
                uri = new Uri($"{SelectedLocation.Path.ToString()}/Map/CheckIn?imei={IMEI}&lat={lat}&lon={lon}&time={tString}&date={dString}");

            var client = new WebClient();

            try
            {
                ResponseString = await client.DownloadStringTaskAsync(uri);
            }
            catch (WebException e)
            {
                var stream = new StreamReader(e.Response.GetResponseStream());
                var msg = await stream.ReadToEndAsync();
                MessageBox.Show(msg);
            }
        }
    }
}
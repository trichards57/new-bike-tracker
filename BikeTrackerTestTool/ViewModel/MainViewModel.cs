using BikeTrackerTestTool.Clients;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
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
        private string responseString = "[Nothing Received]";
        private IClient selectedClient;
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
                     Path = new Uri("https://localhost:44373")
                },
                new ServerLocation
                {
                    Name = "Production",
                    Path = new Uri("http://sjatracker.elasticbeanstalk.com")
                },
                new ServerLocation
                {
                    Name = "App Harbour Production",
                    Path = new Uri("https://sjatracker.apphb.com")
                }
            };

            Clients = new ObservableCollection<IClient>
            {
                //new NokiaClient { Imei = "1234", BaseLatitude = 51.532M, BaseLongitude = -2.552M, FailureRate = 0.1 },
                new AndroidClient { Imei = "1235", BaseLatitude = 51.533M, BaseLongitude = -2.553M, FailureRate = 0.1 }
            };

            SelectedClient = Clients.First();
            SelectedLocation = Locations.First();
            UpdateRate = 15;
            StartUpdate = new RelayCommand(ExecuteStartUpdate, CanExecuteStartUpdate);
            StopUpdate = new RelayCommand(ExecuteStopUpdate, CanExecuteStopUpdate);
        }

        public ObservableCollection<IClient> Clients { get; }
        public ObservableCollection<ServerLocation> Locations { get; }

        public string ResponseString
        {
            get => responseString;
            set => Set(ref responseString, value);
        }

        public IClient SelectedClient
        {
            get => selectedClient;
            set => Set(ref selectedClient, value);
        }

        public ServerLocation SelectedLocation
        {
            get => selectedLocation;
            set => Set(ref selectedLocation, value);
        }

        public RelayCommand StartUpdate { get; }

        public RelayCommand StopUpdate { get; }

        public int UpdateRate
        {
            get => updateRate;
            set => Set(ref updateRate, value);
        }

        public bool UpdateRunning
        {
            get => updateRunning;
            set => updateRunning = value;
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
            var res = Clients.Select(c => c.SendUpdate(SelectedLocation.Path));

            await Task.WhenAll(res.Where(t => t != null));
        }
    }
}
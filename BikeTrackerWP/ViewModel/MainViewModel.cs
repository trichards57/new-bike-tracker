using GalaSoft.MvvmLight;
using System;
using System.Threading;
using Windows.Devices.Geolocation;
using Windows.Networking.Connectivity;

namespace BikeTrackerWP.ViewModel
{
    public enum DataStatus
    {
        None,
        Connected
    }

    public enum LocationStatus
    {
        None,
        Ready
    }

    public enum TrackerStatus
    {
        Stopped,
        Starting,
        Running
    }

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
        private Geolocator locator;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            if (IsInDesignMode)
            {
                DataStatus = DataStatus.Connected;
                LocationStatus = LocationStatus.Ready;
                TrackerStatus = TrackerStatus.Running;
                UpdateRate = 2;
                EnableTracker = true;
                LastUpdate = DateTime.Now;
            }
            else
            {
                NetworkInformation_NetworkStatusChanged(null);
                LocationStatus = LocationStatus.None;
                UpdateRate = 2;
                EnableTracker = false;
                NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;
                locator = new Geolocator();
                locator.StatusChanged += Locator_StatusChanged;
                var res = locator.GetGeopositionAsync();
            }
        }

        private void Locator_StatusChanged(Geolocator sender, StatusChangedEventArgs args)
        {
            LocationStatus = (locator.LocationStatus == PositionStatus.Ready) ? LocationStatus.Ready : LocationStatus.None;
        }

        private void NetworkInformation_NetworkStatusChanged(object sender)
        {
            var profile = NetworkInformation.GetInternetConnectionProfile();
            var status = profile?.GetNetworkConnectivityLevel();
            DataStatus = (status != null && status == NetworkConnectivityLevel.InternetAccess) ? DataStatus.Connected : DataStatus.None;
        }

        private Timer updateTimer;

        private DataStatus dataStatus;
        private LocationStatus locationStatus;
        private TrackerStatus trackerStatus;
        private decimal updateRate;
        private bool enableTracker;
        private DateTime lastUpdate;

        public bool EnableTracker
        {
            get { return enableTracker; }
            set { Set(ref enableTracker, value); }
        }

        public DataStatus DataStatus
        {
            get { return dataStatus; }
            set { Set(ref dataStatus, value); }
        }

        public LocationStatus LocationStatus
        {
            get { return locationStatus; }
            set { Set(ref locationStatus, value); }
        }

        public TrackerStatus TrackerStatus
        {
            get { return trackerStatus; }
            set { Set(ref trackerStatus, value); }
        }

        public decimal UpdateRate
        {
            get { return updateRate; }
            set { Set(ref updateRate, value); }
        }

        public DateTime LastUpdate
        {
            get { return lastUpdate; }
            set { Set(ref lastUpdate, value); }
        }
    }
}
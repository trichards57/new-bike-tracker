using System;

namespace BikeTrackerTestTool.ViewModel
{
    public class ServerLocation
    {
        public string Name { get; set; }
        public Uri Path { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}

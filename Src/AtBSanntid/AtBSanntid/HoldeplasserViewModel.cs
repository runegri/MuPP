using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Diagnostics;

namespace Holdeplasser
{
    public class HoldeplasserViewModel : INotifyPropertyChanged
    {
        private const string MapUriTemplate =
            "http://maps.google.com/maps/api/staticmap?center={0},{1}&zoom=16&size=456x530&sensor=false&markers=color:blue|{0},{1}";


        public event PropertyChangedEventHandler PropertyChanged;

        public HoldeplasserViewModel()
        {
            ReadStopInfo();
        }

        private List<StopInfo> _stops;
        public List<StopInfo> Stops
        {
            get
            {
                return _stops;
            }
            private set
            {
                _stops = value;
                RaisePropertyChanged("Stops");
            }
        }

        private StopInfo _selectedStop;
        public StopInfo SelectedStop
        {
            get
            {
                return _selectedStop;
            }
            set
            {
                _selectedStop = value;
                RaisePropertyChanged("SelectedStop");
                RaisePropertyChanged("SelectedLatitude");
                RaisePropertyChanged("SelectedLongtitude");
                RaisePropertyChanged("SelectedStopName");
                RaisePropertyChanged("MapUri");
            }
        }

        public string SelectedLatitude
        {
            get
            {
                if (SelectedStop == null)
                {
                    return "";
                }
                return SelectedStop.LatLon.Latitude.ToString().Replace(",", ".");
            }
        }

        public string SelectedLongtitude
        {
            get
            {
                if (SelectedStop == null)
                {
                    return "";
                }
                return SelectedStop.LatLon.Longtitude.ToString().Replace(",", ".");
            }
        }
		
        public string SelectedStopName
        {
            get
            {
                if (SelectedStop == null)
                {
                    return "";
                }
                return SelectedStop.StopName;
            }
        }

        public Uri MapUri
        {
            get
            {
                if(SelectedStop == null)
                {
                    return null;
                }
                var uriString = string.Format(MapUriTemplate, SelectedLatitude, SelectedLongtitude);
                return new Uri(uriString, UriKind.Absolute);
            }
        }

        private void ReadStopInfo()
        {
            Stops = new List<StopInfo>();
			
			Debug.WriteLine("Exists? " + File.Exists(Path.Combine(Environment.CurrentDirectory, "Data/R1615.HPL")));
			
			try
			{
	            using (var fileStream = File.Open(Path.Combine(Environment.CurrentDirectory, "Data/R1615.HPL"), FileMode.Open, FileAccess.Read))
	            using (var reader = new StreamReader(fileStream))
	            {
	                while (!reader.EndOfStream)
	                {
	                    var line = reader.ReadLine();
	                    var stopInfo = new StopInfo(line);
	                    Stops.Add(stopInfo);
	                }
	            }
			}
			catch(Exception ex)
			{ 
				Debug.WriteLine(ex.ToString());
				throw; 
			}

        }

        public void RaisePropertyChanged(string propertyName)
        {
            var propertyChanged = PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

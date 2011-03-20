using MonoTouch.CoreLocation;
using MonoTouch.MapKit;
using AtB;

namespace iPhoneUI
{
	public class BusStopMapAnnotation : MKAnnotation
	{
		private BusStop _busStop;
		
		public BusStopMapAnnotation (BusStop busStop)
		{
			_busStop = busStop;	
		}
		
		public override CLLocationCoordinate2D Coordinate 
		{
			get { return _busStop.GetCLLocationCoordinate2D(); }
			set { }
		}
		
		public override string Title 
		{
			get { return _busStop.Name;	}
		}
	}
}

using MonoTouch.CoreLocation;
using AtB;

namespace iPhoneUI
{
	/// <summary>
	/// This class is used to add extensions to the BusStop class that are iOS specific, 
	/// hence not cross-plattform.
	/// </summary>
	public static class BusStopExtensions
	{
		public static CLLocationCoordinate2D GetCLLocationCoordinate2D(this BusStop busStop)
		{
			return new CLLocationCoordinate2D(busStop.Latitude, busStop.Longtitude);
		}
	}
}

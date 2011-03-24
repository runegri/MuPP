using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreLocation;
using MonoTouch.MapKit;
using System.Threading;
using System.Drawing;
using System.Text;
using AtB;
using MonoTouch.MessageUI;
using System.IO;

namespace iPhoneUI
{
	public class MapViewController : UIViewController
	{
		private MKMapView _map;
		private BusStop[] _busStops;
		
		public MapViewController (string title, params BusStop[] busStops)
		{
			_busStops = busStops;
			this.Title = title;
		}
		
		public override void ViewDidLoad ()
		{
			_map = new MKMapView();			
			
			foreach(BusStop busStop in _busStops)
			{
				AddBusStopToMap(_map, busStop);
			}	
			
			AddBusStopToMap(_map, _busStops[0]);
			
			_map.ShowsUserLocation = true;
			
			_map.Frame = new RectangleF (0, 0, this.View.Bounds.Width, this.View.Bounds.Height);
			
			if(_busStops.Length <= 1)
			{
				_map.Region = new MKCoordinateRegion(FindCentre(_busStops), new MKCoordinateSpan(0.005, 0.0005));
			}
			else
			{
				_map.Region = RegionThatFitsAllStops(_busStops);
			}
			
			this.View.AddSubview (_map);
		}
		
		private MKCoordinateRegion RegionThatFitsAllStops(IList<BusStop> busStops)
		{
			var minLatitude = double.MaxValue;
			var maxLatitude = double.MinValue;
			var minLongtitude = double.MaxValue;
			var maxLongtitude = double.MinValue;
			
			foreach(var busStop in busStops)
			{				
				minLatitude = Math.Min(minLatitude, busStop.Latitude);
				maxLatitude = Math.Max(maxLatitude, busStop.Latitude);
				minLongtitude = Math.Min(minLongtitude, busStop.Longtitude);
				maxLongtitude = Math.Max(maxLongtitude, busStop.Longtitude);				
			}
			
			var centerLatitude = (minLatitude + maxLatitude) / 2;
			var centerLongtitude = (minLongtitude + maxLongtitude) / 2;
			
			var dLatitude = (maxLatitude - minLatitude) * 1.1;
			var dLongtitude = (maxLongtitude - minLongtitude) * 1.1;
			
			return new MKCoordinateRegion(
                      new CLLocationCoordinate2D(centerLatitude, centerLongtitude), 
                      new MKCoordinateSpan(dLatitude, dLongtitude));
			
		}
		
		// Something for Applicable
		private CLLocationCoordinate2D FindCentre(IList<BusStop> busStops) 
		{
			double latitude = busStops.Average(bs => bs.Latitude);
			double longitude = busStops.Average(bs => bs.Longtitude);
			return new CLLocationCoordinate2D(latitude, longitude);
		}
		
		private void AddBusStopToMap(MKMapView map, BusStop busStop)
		{
			BusStopMapAnnotation annotation = new BusStopMapAnnotation(busStop);
			map.AddAnnotation(annotation);
		}
		
	}
	
	

	

}

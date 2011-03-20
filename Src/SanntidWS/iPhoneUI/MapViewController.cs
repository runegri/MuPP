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
		private BusStop _busStop;
		
		public MapViewController (string title, BusStop stopInfo)
		{
			_busStop = stopInfo;
			this.Title = title;
		}

		public override void ViewDidLoad ()
		{
			_map = new MKMapView();			
			
			AddBusStopToMap(_map, _busStop);
						
			_map.Frame = new RectangleF (0, 0, this.View.Bounds.Width, this.View.Bounds.Height);
			
			_map.Region = new MKCoordinateRegion(_busStop.GetCLLocationCoordinate2D(), new MKCoordinateSpan(0.005, 0.0005));
			
			this.View.AddSubview (_map);
		}
		
		
		private void AddBusStopToMap(MKMapView map, BusStop busStop)
		{
			BusStopMapAnnotation annotation = new BusStopMapAnnotation(_busStop);
			map.AddAnnotation(annotation);
		}
	}
	
	

	

}

using System;
using MonoTouch.MapKit;
using MonoTouch.CoreLocation;
namespace AtBSanntid
{
	public class BusStopMarker : MKAnnotation
	{
		#region implemented abstract members of MonoTouch.MapKit.MKAnnotation
		private CLLocationCoordinate2D _location;
		public override CLLocationCoordinate2D Coordinate
		{
			get { return _location; }
			set { _location = value; }
		}
		
		private string _title;
		public override string Title
		{
			get { return _title; }
		}
		
		public override string Subtitle
		{
			get { return ""; }
		}
		
		#endregion
		public BusStopMarker (string title, CLLocationCoordinate2D coordinate) 
		{
			_title = title;
			Coordinate = coordinate;
		}
		
	}
}


using System;
using MonoTouch.UIKit;
using System.Drawing;
using AtB;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using TinyIoC;
using GpsTool;

namespace SanntidWS
{
	public class SanntidView : UIViewController
	{
		private UIButton _loadDataButton;
			
		private UITextView _result;
		
		private UIActivityIndicatorView _activityIndicator;
		
		private Sanntid _sanntid;
		private IGpsService _gpsService;
		
		private List<BusStop> _stops = new List<BusStop>();
		
		private bool _locationObtained;
		
		public SanntidView ()
		{
			_sanntid = TinyIoCContainer.Current.Resolve<Sanntid>(); 
			_gpsService = TinyIoCContainer.Current.Resolve<IGpsService>();
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			_loadDataButton = UIButton.FromType(UIButtonType.RoundedRect);
			_loadDataButton.SetTitle("Hent sanntidsdata", UIControlState.Normal);
			_loadDataButton.Frame = new RectangleF(10, 10, View.Bounds.Width - 20, 50);
				
			_result = new UITextView(new RectangleF(10, 70, View.Bounds.Width - 20, View.Bounds.Height - 80));
			_result.Font = UIFont.FromName("Arial", 14);
			_result.Editable = false;
			
			_activityIndicator = new UIActivityIndicatorView(new RectangleF(View.Bounds.Width / 2 - 20, View.Bounds.Height / 2 - 20, 40, 40));	
			_activityIndicator.AutoresizingMask = UIViewAutoresizing.FlexibleBottomMargin | UIViewAutoresizing.FlexibleTopMargin | UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin;
			_activityIndicator.ActivityIndicatorViewStyle = UIActivityIndicatorViewStyle.WhiteLarge;
		
			View.AddSubview(_activityIndicator);
			View.AddSubview(_loadDataButton);
			View.AddSubview(_result);
			
			View.BackgroundColor = UIColor.DarkGray;
			
			_loadDataButton.TouchUpInside += delegate(object sender, EventArgs e) {
				_activityIndicator.StartAnimating();
				if(!_stops.Any())
				{
					_loadDataButton.SetTitle("Laster holdeplasser", UIControlState.Normal);
					_loadDataButton.Enabled = false;
					ThreadPool.QueueUserWorkItem(o => _sanntid.GetBusStopList(BusStopsLoaded));
				}
				else
				{
					GetLocation();
				}
			};
			
		}
			
		private void BusStopsLoaded(List<BusStop> busStops)
		{
			_stops.Clear();
			_stops.AddRange(busStops);	
			GetLocation();
			
			InvokeOnMainThread(() =>
           	{
				_activityIndicator.StopAnimating();
				_loadDataButton.Enabled = false;
				_loadDataButton.SetTitle("Oppdater sanntidsdata", UIControlState.Normal);
			});
			
		}
		
		private void GetLocation()
		{
			_locationObtained = false;
			_gpsService.LocationChanged = LocationObtained;
			_gpsService.Start();
		}
		
		private void DisplayRealTimeData(BusStop stop, List<StopTime> stopTimes)
		{
			
			var sb = new StringBuilder();
			sb.AppendLine("Sanntidsdata for " + stop.Name);
			sb.AppendLine();
			foreach(var stopTime in stopTimes)
			{
				sb.AppendLine(stopTime.ToString());
			}
			
			if(!stopTimes.Any())
			{
				sb.AppendLine("Fant ingen data for holdeplassen.");
			}

			InvokeOnMainThread(() => 
			{
				_result.Text += Environment.NewLine + sb.ToString();
				_activityIndicator.StopAnimating();
				_loadDataButton.Enabled = true;
			});
			
		}
		
		private void LocationObtained(LocationData location)
		{
			
			if (!_locationObtained && location.Latitude > 1 && location.Longtitude > 1)
			{
				_locationObtained = true;
				_gpsService.Stop();
				
				var currentCoordinate = new GeographicCoordinate(location.Latitude, location.Longtitude);
				
				var closestStop = _stops.OrderBy(s => RelativeDistance(s.Location, currentCoordinate)).First();

				var text = "Fant posisjon: " + currentCoordinate + 
					Environment.NewLine + "Nermeste holdeplass: " + closestStop.Name + " " + closestStop.Location + 
					Environment.NewLine + "Laster sanntidsdata..." + Environment.NewLine;
				
				InvokeOnMainThread(() => _result.Text += text);
				
				ThreadPool.QueueUserWorkItem(o => _sanntid.GetRealTimeData(closestStop, stops => DisplayRealTimeData(closestStop, stops)));
				
				
			}
			
		}		
		
		private double RelativeDistance(GeographicCoordinate c1, GeographicCoordinate c2)
		{
			if( c1 == null || c2 == null)
			{
				return double.MaxValue;
			}
		
			var dlat = c1.Latitude - c2.Latitude;
			var dlon = c1.Longtitude - c2.Longtitude;
			
			return Math.Sqrt(dlat * dlat + dlon * dlon);	
		}
				                        
				                       
	}
}


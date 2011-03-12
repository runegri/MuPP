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
using MonoTouch.Foundation;

namespace SanntidWS
{
	[Preserve(AllMembers=true)]
	public class SanntidView : UIViewController
	{
		private UIButton _loadDataButton;
			
		private UITextView _result;
		
		private UIActivityIndicatorView _activityIndicator;
		
		private Sanntid _sanntid;
		private IGpsService _gpsService;
		
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
				_result.Text = "Jobber..." + Environment.NewLine + Environment.NewLine;
				ThreadPool.QueueUserWorkItem(o => _sanntid.GetBusStopList(BusStopsLoaded));	
			};
			
		}
			
		private void BusStopsLoaded(List<BusStop> busStops)
		{
			GetLocation(busStops);
			
			InvokeOnMainThread(() =>
           	{
				_activityIndicator.StopAnimating();
		   	});
			
		}
		
		private void GetLocation(List<BusStop> busStops)
		{
			_gpsService.LocationChanged = location => LocationObtained(location, busStops);
			_gpsService.Start();
		}
		
		private void DisplayRealTimeData(BusStop stop, List<StopTime> stopTimes)
		{
			
			var sb = new StringBuilder();
			sb.AppendLine("Sanntidsdata for " + stop.Name);
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
			});
			
		}
		
		private void LocationObtained(LocationData location, List<BusStop> busStops)
		{
			
			if (location.Latitude > 1 && location.Longtitude > 1)
			{
				
				_gpsService.LocationChanged = null;
				
				var currentCoordinate = new GeographicCoordinate(location.Latitude, location.Longtitude);
					
				var closestStops = busStops.OrderBy(s => RelativeDistance(s.Location, currentCoordinate)).Take(5).ToList();

				foreach(var closestStop in closestStops)
				{
					GetRealTimeDataForBusStop(closestStop);
				}	
			}
			
		}		
		
		private void GetRealTimeDataForBusStop(BusStop busStop)
		{
			ThreadPool.QueueUserWorkItem(o => _sanntid.GetRealTimeData(busStop, stops => DisplayRealTimeData(busStop, stops)));
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


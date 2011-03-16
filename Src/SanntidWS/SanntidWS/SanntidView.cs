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
		
		private Realtime _sanntid;
		private IGpsService _gpsService;
		
		private LocationData _location;
		
		public SanntidView ()
		{
			_sanntid = TinyIoCContainer.Current.Resolve<Realtime>(); 
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
				if(_location != null)
				{
					_activityIndicator.StartAnimating();
					_result.Text = "Jobber..." + Environment.NewLine + Environment.NewLine;
					var coordinate = new GeographicCoordinate(_location.Latitude, _location.Longtitude);
					ThreadPool.QueueUserWorkItem(o => _sanntid.GetNearbyStops(coordinate, BusStopsLoaded));	
				}
			};

			_gpsService.LocationChanged = location => _location = location; 
			_gpsService.Start();
			
		}
			
		private void BusStopsLoaded(List<BusStop> closestStops)
		{
			foreach(var closestStop in closestStops)
			{
				GetRealTimeDataForBusStop(closestStop);
			}
		}

		private void GetRealTimeDataForBusStop(BusStop busStop)
		{
			ThreadPool.QueueUserWorkItem(o => 
                 _sanntid.GetRealTimeData(busStop, stops => DisplayRealTimeData(busStop, stops)));
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
			                        		                       
	}
}


using System;
using MonoTouch.UIKit;
using System.Drawing;
using AtB;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SanntidWS
{
	public class SanntidView : UIViewController
	{
		private UIButton _loadDataButton;
		private UIButton _loadRealTimeButton;
		
		private UITextField _stopName;
		
		private UITextView _result;
		
		private Sanntid _sanntid;
		
		private List<BusStop> _stops = new List<BusStop>();
		
		public SanntidView ()
		{
			_sanntid = new Sanntid();
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			_loadDataButton = UIButton.FromType(UIButtonType.RoundedRect);
			_loadDataButton.SetTitle("Load stops", UIControlState.Normal);
			_loadDataButton.Frame = new RectangleF(10, 10, View.Bounds.Width - 20, 50);
			
			_stopName = new UITextField(new RectangleF(10,70, 100, 50));
			_stopName.BorderStyle = UITextBorderStyle.RoundedRect;
			
			_loadRealTimeButton = UIButton.FromType(UIButtonType.RoundedRect);
			_loadRealTimeButton.SetTitle("Load real-time data", UIControlState.Normal);
			_loadRealTimeButton.Frame = new RectangleF(120, 70, View.Bounds.Width - 130, 50);
			_loadRealTimeButton.Enabled = false;
			
			
			_result = new UITextView(new RectangleF(0, 130, View.Bounds.Width, View.Bounds.Height - 110));
			_result.Font = UIFont.FromName("Arial", 14);

			View.AddSubview(_loadDataButton);
			View.AddSubview(_loadRealTimeButton);
			View.AddSubview(_stopName);
			View.AddSubview(_result);
			
			View.BackgroundColor = UIColor.DarkGray;
			
			_loadDataButton.TouchUpInside += delegate(object sender, EventArgs e) {
				_sanntid.GetBusStopList(BusStopsLoaded);
			};
			
			_loadRealTimeButton.TouchUpInside += (s,e) =>
			{
				var selectedStop = _stops.FirstOrDefault(stop => stop.Name.Contains(_stopName.Text));
				if (selectedStop != null)
				{
					_sanntid.GetRealTimeData(selectedStop, stops => DisplayRealTimeData(selectedStop, stops));
				}
			};
			
			_stopName.ShouldReturn = t => {
				t.ResignFirstResponder();
				return true;
			};
		}
			
		private void BusStopsLoaded(List<BusStop> busStops)
		{
			_stops.AddRange(busStops);			
			_loadRealTimeButton.Enabled = true;
			var stop = _stops.First();
			
			InvokeOnMainThread(() => _stopName.Text = stop.Name);
			
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

			InvokeOnMainThread(() => _result.Text = sb.ToString());
			
		}
				                        
				                       
	}
}


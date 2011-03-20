
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
	public class BusStopViewController : UIViewController
	{
		public UITextView textView;

		public UIView _view;

		public UIWebView webView;

		private BusStop _stopInfo;

		private IBusStopRepository _busStopRepository = TinyIoC.TinyIoCContainer.Current.Resolve<IBusStopRepository> ();
		
	//	private MFMailComposeViewController _mail;
		
		public BusStopViewController (BusStop stopInfo)
		{
			_stopInfo = stopInfo;
			_busStopRepository.AddMostRecent (_stopInfo);
			this.Title = _stopInfo.Name;
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			_view = new UIView ();
			_view.BackgroundColor = UIColor.GroupTableViewBackgroundColor;
			
			UILabel label = new UILabel ();
			label.Text = _stopInfo.Name;
			label.BackgroundColor = UIColor.GroupTableViewBackgroundColor;
			label.SizeToFit ();
			label.Frame = new RectangleF (10, 5, 300, 40);
			
			_view.Add (label);
			
			UIButton button = UIButton.FromType (UIButtonType.RoundedRect);
			button.SetTitle ("Legg til i favoritter", UIControlState.Normal);			
			button.Frame = new RectangleF (7, 45, 148, 50);
			button.TouchUpInside += delegate { _busStopRepository.AddFavorite (_stopInfo); };			
			_view.Add (button);
			
			_view.SizeToFit ();
			
			
			UIButton mapButton = UIButton.FromType (UIButtonType.RoundedRect);
			mapButton.SetTitle ("Se kart", UIControlState.Normal);			
			mapButton.Frame = new RectangleF (165, 45, 148, 50);
			//button.TouchUpInside += delegate { _busStopRepository.AddFavorite (_stopInfo); };			
			_view.Add (mapButton);
						
			// Reposition and resize the receiver
			_view.Frame = new RectangleF (0, 0, this.View.Bounds.Width, this.View.Bounds.Height);
			
			// Add the table view as a subview
			this.View.AddSubview (_view);
			
			_busStopRepository.GetRealTimeData (_stopInfo, RealTimeDataLoaded);
		}

		private void RealTimeDataLoaded (List<StopTime> stops)
		{
			
			InvokeOnMainThread (() =>
			{
				var y = 140;
				if (stops.Any ()) 
				{
					foreach (var stop in stops) {
						UILabel label = new UILabel ();
						label.Text = stop.ToString ();
						label.BackgroundColor = UIColor.GroupTableViewBackgroundColor;
						label.Frame = new RectangleF (10, y, 300, 30);
						_view.Add (label);
						y += 35;
					}
				} 
				else 
				{
					UILabel label = new UILabel ();
					label.Text = "Fant ingen data for holdeplassen";
					label.BackgroundColor = UIColor.GroupTableViewBackgroundColor;
					label.Frame = new RectangleF (10, y, 300, 30);
					_view.Add (label);
				}
			});
		}

	}
}

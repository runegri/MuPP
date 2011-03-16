
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

namespace iPhoneUI
{
	public class BusStopViewController : UIViewController
	{
		public UITextView textView;
		
		public UIView _view;
		
		public UIWebView webView;
		
		private BusStop _stopInfo;
		
		private IBusStopRepository _busStopRepository = TinyIoC.TinyIoCContainer.Current.Resolve<IBusStopRepository>();
		
		public BusStopViewController (BusStop stopInfo)
		{
			_stopInfo = stopInfo;
			_busStopRepository.AddMostRecent(_stopInfo);
			this.Title = _stopInfo.Name;
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			_view = new UIView();
			_view.BackgroundColor = UIColor.GroupTableViewBackgroundColor;
			
			UILabel label = new UILabel();
			label.Text = _stopInfo.Name;
			label.BackgroundColor = UIColor.GroupTableViewBackgroundColor;
			label.SizeToFit();
			label.Frame = new RectangleF(10,0, 300, 40);
			
			_view.Add(label);
			
			
			UIButton button = UIButton.FromType(UIButtonType.RoundedRect);
			button.SetTitle("Legg til i favoritter", UIControlState.Normal);;
			
			button.Frame = new RectangleF(10, 60, 150, 50);
			
			_view.Add(button);
			
			_view.SizeToFit();
			
			button.TouchUpInside += delegate {
				_busStopRepository.AddFavorite(_stopInfo);
			};
			
			webView = new UIWebView { ScalesPageToFit = false };
			
			
			webView.LoadHtmlString (FormatText (), new NSUrl ());
			
			// Set the web view to fit the width of the app.
			webView.SizeToFit ();
			
			// Reposition and resize the receiver
			_view.Frame = new RectangleF (0, 0, this.View.Bounds.Width, this.View.Bounds.Height);
			
			// Add the table view as a subview
			this.View.AddSubview (_view);
			
			_busStopRepository.GetRealTimeData(_stopInfo, RealTimeDataLoaded);	
		}
		
		private void RealTimeDataLoaded(List<StopTime> stops) 
		{
			
			InvokeOnMainThread(() => {
				var y = 100;
				foreach(var stop in stops)
				{
					UILabel label = new UILabel();
					label.Text = stop.ToString();
					label.BackgroundColor = UIColor.GroupTableViewBackgroundColor;
					label.Frame = new RectangleF(10,y, 300, 30);
					_view.Add(label);
					y += 35;
				}}
			);
		}
		
		private string FormatText ()
		{
			StringBuilder sb = new StringBuilder ();
			
			sb.Append (@"<style>body,b,p{font-family:Helvetica;font-size:14px}</style>");
						
			sb.Append ("<p>Holdeplass navn: " + _stopInfo.Name + "</p>" + Environment.NewLine);
			sb.Append ("<p>Holdeplass nr: " + _stopInfo.StopCode + "</p>" + Environment.NewLine);
			sb.Append ("<p>Latitude: " + _stopInfo.Latitude + "</p>" + Environment.NewLine);
			sb.Append ("<p>Longitude: " +  _stopInfo.Latitude + "</p>" + Environment.NewLine);
			
			return sb.ToString ();
		}
	}
}

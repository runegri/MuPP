
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
using Core;
namespace iPhoneUI
{
	public class BusStopViewController : UIViewController
	{
		public UITextView textView;
		public UIWebView webView;
		
		private StopInfo _stopInfo;
		
		public BusStopViewController (StopInfo stopInfo)
		{
			_stopInfo = stopInfo;
			this.Title = _stopInfo.StopName;
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// no XIB !
			webView = new UIWebView { ScalesPageToFit = false };
			webView.LoadHtmlString (FormatText (), new NSUrl ());
			
			// Set the web view to fit the width of the app.
			webView.SizeToFit ();
			
			// Reposition and resize the receiver
			webView.Frame = new RectangleF (0, 0, this.View.Bounds.Width, this.View.Bounds.Height);
			
			// Add the table view as a subview
			this.View.AddSubview (webView);
			
		}

		private string FormatText ()
		{
			StringBuilder sb = new StringBuilder ();
			
			sb.Append (@"<style>
body,b,p{font-family:Helvetica;font-size:14px}
</style>");
						
			sb.Append ("<p>" + _stopInfo.StopName + "</p>" + Environment.NewLine);
			sb.Append ("<p>" + _stopInfo.StopNumber + "</p>" + Environment.NewLine);
			sb.Append ("<p>" + _stopInfo.LatLon.X + "," + _stopInfo.LatLon.Y + "</p>" + Environment.NewLine);
			
			return sb.ToString ();
		}
	}
}

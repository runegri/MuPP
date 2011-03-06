
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreLocation;
using MonoTouch.MapKit;
using Holdeplasser;
using System.Threading;

namespace AtBSanntid
{
	public class Application
	{
		static void Main (string[] args)
		{
			UIApplication.Main (args);
		}
	}

	// The name AppDelegate is referenced in the MainWindow.xib file.
	public partial class AppDelegate : UIApplicationDelegate
	{
		
		private HoldeplasserViewModel _viewModel;
		
		// This method is invoked when the application has loaded its UI and its ready to run
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			// If you have defined a view, add it here:
			// window.AddSubview (navigationController.View);
		
			ThreadPool.QueueUserWorkItem(o => LoadStops());
			
			BusStopCode.ShouldReturn = delegate { 
				BusStopCode.ResignFirstResponder(); 
				SelectStopWithCode();
				return true; 
			};
			
			window.MakeKeyAndVisible ();
			
			return true;
		}
		
		private void LoadStops()
		{
			_viewModel = new HoldeplasserViewModel();
			
			var selectedStop = _viewModel.Stops.First();
		
			InvokeOnMainThread(() => SelectStop(selectedStop));
		
		}

		// This method is required in iPhoneOS 3.0
		public override void OnActivated (UIApplication application)
		{
		}
		
		private void SelectStop(StopInfo selectedStop)
		{
			var center = new CLLocationCoordinate2D(selectedStop.LatLon.Latitude, selectedStop.LatLon.Longtitude);
			var region = new MKCoordinateRegion(center, new MKCoordinateSpan(0.003, 0.003));
			var marker = new BusStopMarker(selectedStop.StopName, center);
			
			BusStopMap.SetRegion(BusStopMap.RegionThatFits(region), true);
			if(BusStopMap.Annotations.Length > 0)
			{
				var annotation = (MKAnnotation)BusStopMap.Annotations[0];
				BusStopMap.RemoveAnnotation(annotation);
			}
			BusStopMap.AddAnnotationObject(marker);
		
			BusStopCode.Text = selectedStop.StopName;			
		}
		
		private void SelectStopWithCode()
		{
			ActivityIndicator.StartAnimating();
			var stopNumber = BusStopCode.Text.ToLowerInvariant();
			var foundStop = _viewModel.Stops.FirstOrDefault(s => s.StopName.ToLowerInvariant().Contains(stopNumber));
			if(foundStop != null)
			{
				SelectStop(foundStop);
			}
			ActivityIndicator.StopAnimating();
		}
		
		
		
	}
}


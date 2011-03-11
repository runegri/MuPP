
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using TinyIoC;
using GpsTool;

namespace SanntidWS
{
	public class Application
	{
		static void Main (string[] args)
		{
			SetupIoCContainer();
			UIApplication.Main (args);
		}
		
		private static void SetupIoCContainer()
		{
#if SIMULATOR
			TinyIoCContainer.Current.Register<IGpsService, DebugGpsService>();
#else
			TinyIoCContainer.Current.Register<IGpsService, IOSGpsService>();
#endif
			TinyIoCContainer.Current.Register<SanntidView>();
		}
	}

	// The name AppDelegate is referenced in the MainWindow.xib file.
	public partial class AppDelegate : UIApplicationDelegate
	{
		
		private SanntidView _sanntidView;
		
		// This method is invoked when the application has loaded its UI and its ready to run
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			
			_sanntidView = TinyIoCContainer.Current.Resolve<SanntidView>();
			
			// If you have defined a view, add it here:
			window.AddSubview (_sanntidView.View);
			
			window.MakeKeyAndVisible ();
			
			return true;
		}

		// This method is required in iPhoneOS 3.0
		public override void OnActivated (UIApplication application)
		{
		}
	}
}


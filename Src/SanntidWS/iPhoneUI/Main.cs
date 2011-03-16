
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using AtB;
using TinyIoC;
using GpsTool;

namespace iPhoneUI
{
	public class Application
	{
		static void Main (string[] args)
		{
			TinyIoCContainer.Current.Register<IBusStopRepository, BusStopRepository>();
//#if DEBUG
			//TinyIoCContainer.Current.Register<IGpsService, DebugGpsService>();
//#else
			TinyIoCContainer.Current.Register<IGpsService, IOSGpsService>();
//#endif
			UIApplication.Main(args, null, "AppDelegate");
			//TinyIoCContainer.Current.AutoRegister();
			//TinyIoC.TinyIoCContainer.Current.Register<FavoritesViewController>();
		}
	}
		
	[Register("AppDelegate")]
	public partial class AppDelegate2 : UIApplicationDelegate
	{
		UIWindow _window;
	    
		TabBarController _tabBarController;
					
		public override bool FinishedLaunching (UIApplication application, NSDictionary launcOptions)
		{
			_window = new UIWindow(UIScreen.MainScreen.Bounds);
			
			_tabBarController = new TabBarController();
			//_tabBarController.Delegate = new MainTabBarControllerDelegate();
						
			_window.AddSubview(_tabBarController.View);
			_window.MakeKeyAndVisible();
			return true;
		}
		
		// This method is required in iPhoneOS 3.0
		public override void OnActivated (UIApplication application)
		{
			
		}
	}
	
}


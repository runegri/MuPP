
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace iPhoneUI
{
	public class Application
	{
		static void Main (string[] args)
		{
			UIApplication.Main(args, null, "AppDelegate");
		}
	}
		
	[Register("AppDelegate")]
	public partial class AppDelegate2 : UIApplicationDelegate
	{
		UIWindow _window;
	    
		MainTabBarController _tabBarController;
					
		public override bool FinishedLaunching (UIApplication application, NSDictionary launcOptions)
		{
			_window = new UIWindow(UIScreen.MainScreen.Bounds);
			
			_tabBarController = new MainTabBarController();
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


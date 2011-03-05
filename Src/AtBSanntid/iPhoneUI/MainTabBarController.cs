using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreLocation;
using MonoTouch.MapKit;
using System.Threading;

namespace iPhoneUI
{
	public class MainTabBarController : UITabBarController
	{
		AllStopsViewController _allStopsViewController;
		
		EmptyViewController _mainViewController1, _mainViewController2; 
		
		public override void ViewDidLoad ()
		{
			
			_mainViewController1 = new EmptyViewController();
			_mainViewController1.Title = "Favoritter";
			_mainViewController1.TabBarItem = new UITabBarItem(UITabBarSystemItem.Favorites,0);
			
			_mainViewController2 = new EmptyViewController();
			_mainViewController2.TabBarItem = new UITabBarItem(UITabBarSystemItem.MostRecent,1);
			
			_allStopsViewController = new AllStopsViewController();
			
			UINavigationController allStopsNavController = new UINavigationController();
			allStopsNavController.TabBarItem = new UITabBarItem("Holdeplasser", null, 2);			
			allStopsNavController.PushViewController(_allStopsViewController, false);
			
			var tablist = new UIViewController[] 
			{
			   _mainViewController1, _mainViewController2, allStopsNavController	
			};
			
			ViewControllers = tablist;
		}		
	}
}

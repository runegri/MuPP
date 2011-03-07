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
	public class TabBarController : UITabBarController
	{
		private AllStopsViewController _allStopsViewController;		
		private EmptyViewController _favoritesViewController;
	    private EmptyViewController _recentViewController; 
		
		public override void ViewDidLoad ()
		{
			_favoritesViewController = new EmptyViewController();
			_favoritesViewController.Title = "Favoritter";
			_favoritesViewController.TabBarItem = new UITabBarItem(UITabBarSystemItem.Favorites,0);
			
			_recentViewController = new EmptyViewController();
			_recentViewController.TabBarItem = new UITabBarItem(UITabBarSystemItem.MostRecent,1);
			
			_allStopsViewController = new AllStopsViewController();
			
			UINavigationController allStopsNavController = new UINavigationController();
			allStopsNavController.TabBarItem = new UITabBarItem("Holdeplasser", null, 2);			
			allStopsNavController.PushViewController(_allStopsViewController, false);
			
			var tablist = new UIViewController[] 
			{
			   _favoritesViewController, _recentViewController, allStopsNavController	
			};
			
			ViewControllers = tablist;
		}		
	}
}

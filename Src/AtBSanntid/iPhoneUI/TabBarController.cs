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
		private FavoritesViewController _favoritesViewController;
	    private MostRecentViewController _mostRecentViewController; 
		
		public override void ViewDidLoad ()
		{
			_favoritesViewController = new FavoritesViewController();
			
			UINavigationController favoritesNavController = new UINavigationController();
			favoritesNavController.TabBarItem = new UITabBarItem(UITabBarSystemItem.Favorites, 0);			
			favoritesNavController.PushViewController(_favoritesViewController, false);

			_mostRecentViewController = new MostRecentViewController();
			UINavigationController mostRecentNavController = new UINavigationController();
			mostRecentNavController.TabBarItem = new UITabBarItem(UITabBarSystemItem.MostRecent, 1);			
			mostRecentNavController.PushViewController(_mostRecentViewController, false);
			
			_allStopsViewController = new AllStopsViewController();
			
			UINavigationController allStopsNavController = new UINavigationController();
			allStopsNavController.TabBarItem = new UITabBarItem("Holdeplasser", null, 2);			
			allStopsNavController.PushViewController(_allStopsViewController, false);
			
			var tablist = new UIViewController[] 
			{
			   favoritesNavController, mostRecentNavController, allStopsNavController	
			};
			
			ViewControllers = tablist;
		}		
	}
}

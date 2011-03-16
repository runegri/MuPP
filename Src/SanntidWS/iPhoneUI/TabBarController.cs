using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreLocation;
using MonoTouch.MapKit;
using System.Threading;
using TinyIoC;

namespace iPhoneUI
{
	public class TabBarController : UITabBarController
	{
		private AllStopsViewController _allStopsViewController;		
		private FavoritesViewController _favoritesViewController;
	    private MostRecentViewController _mostRecentViewController; 
		
		public override void ViewDidLoad ()
		{
			_favoritesViewController = TinyIoCContainer.Current.Resolve<FavoritesViewController>();
			
			UINavigationController favoritesNavController = new UINavigationController();
			favoritesNavController.TabBarItem = new UITabBarItem(UITabBarSystemItem.Favorites, 0);			
			favoritesNavController.PushViewController(_favoritesViewController, false);

			_mostRecentViewController = TinyIoCContainer.Current.Resolve<MostRecentViewController>();

			UINavigationController mostRecentNavController = new UINavigationController();
			mostRecentNavController.TabBarItem = new UITabBarItem(UITabBarSystemItem.MostRecent, 1);			
			mostRecentNavController.PushViewController(_mostRecentViewController, false);
			
			_allStopsViewController = TinyIoCContainer.Current.Resolve<AllStopsViewController>();
			
			UINavigationController allStopsNavController = new UINavigationController();
			allStopsNavController.TabBarItem = new UITabBarItem("Alle stopp", null, 2);			
			allStopsNavController.PushViewController(_allStopsViewController, false);
			
			var tablist = new UIViewController[] 
			{
			   favoritesNavController, mostRecentNavController, allStopsNavController	
			};
			
			ViewControllers = tablist;
		}		
	}
}

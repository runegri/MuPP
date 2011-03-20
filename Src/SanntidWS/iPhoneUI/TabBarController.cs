using MonoTouch.UIKit;
using TinyIoC;

namespace iPhoneUI
{
	public class TabBarController : UITabBarController
	{
		private AllStopsViewController _allStopsViewController;		
		private FavoritesViewController _favoritesViewController;
	    private MostRecentViewController _mostRecentViewController;
		private NearbyViewController _nearbyViewController;
		
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

			_nearbyViewController = TinyIoCContainer.Current.Resolve<NearbyViewController>();

			UINavigationController nearbyNavController = new UINavigationController();
			nearbyNavController.TabBarItem = new UITabBarItem("Nær deg", null, 2);			
			nearbyNavController.PushViewController(_nearbyViewController, false);
			
			_allStopsViewController = TinyIoCContainer.Current.Resolve<AllStopsViewController>();
			
			UINavigationController allStopsNavController = new UINavigationController();
			allStopsNavController.TabBarItem = new UITabBarItem("Alle", null, 3);			
			allStopsNavController.PushViewController(_allStopsViewController, false);
			
			var tablist = new UIViewController[] 
			{
			   favoritesNavController, mostRecentNavController, nearbyNavController, allStopsNavController	
			};
			
			ViewControllers = tablist;
		}		
	}
}

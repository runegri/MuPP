using MonoTouch.UIKit;
using TinyIoC;

namespace iPhoneUI
{
	public class TabBarController : UITabBarController
	{
		private int _tabCounter = 0;
		
		public override void ViewDidLoad ()
		{
			UINavigationController favoritesNavController = CreateNavigationController<FavoritesViewController>("Favoritter", "28-star.png");
			
			UINavigationController mostRecentNavController = CreateNavigationController<MostRecentViewController>("Sist brukte", "104-index-cards.png");
			
			UINavigationController nearbyNavController = CreateNavigationController<NearbyViewController>("Nær deg", "71-compass.png");
			
			UINavigationController allStopsNavController = CreateNavigationController<AllStopsViewController>("Alle", "43-bus.png");
		
			ViewControllers = new UIViewController[] 
			{
			   favoritesNavController, mostRecentNavController, nearbyNavController, allStopsNavController	
			};
		}
		
		private UINavigationController CreateNavigationController<T>(string title, string imageName) where T : UIViewController
		{
			UIViewController viewController = TinyIoCContainer.Current.Resolve<T>();

			UINavigationController navController = new UINavigationController();
			navController.TabBarItem = new UITabBarItem(title, UIImage.FromFile(@"Images/" + imageName), _tabCounter++);			
			navController.PushViewController(viewController, false);
			
			return navController;
		}
	}
}

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
		MainViewController _mainViewController1, _mainViewController2, _mainViewController3;
		
		public override void ViewDidLoad ()
		{
			_mainViewController1 = new MainViewController();
			_mainViewController1.Title = "Favoritter";
			_mainViewController1.TabBarItem = new UITabBarItem(UITabBarSystemItem.Favorites,0);
			
			_mainViewController2 = new MainViewController();
			_mainViewController2.TabBarItem = new UITabBarItem(UITabBarSystemItem.MostRecent,0);
			
			_mainViewController3 = new MainViewController();
			_mainViewController3.TabBarItem = new UITabBarItem("Holdeplasser", null, 0);
			
			var tablist = new UIViewController[] 
			{
			   _mainViewController1, _mainViewController2, _mainViewController3	
			};
			
			ViewControllers = tablist;
		}		
	}
}


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
	public class NavigationController : UINavigationController 
	{
		public override void ViewDidLoad ()
		{
			MainTabBarController table = new MainTabBarController();			
			SetViewControllers(new UIViewController[] {table},false);
			
			base.ViewDidLoad ();
		}
    }
}

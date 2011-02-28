
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
			UIApplication.Main(args, "TabBar02", "AppDelegate2");
		}
	}
	
	[Register("TabBar02")]
	public class TabBar02 : UIApplication {}
	
	[Register("AppDelegate2")]
	public partial class AppDelegate2 : UIApplicationDelegate {}

}


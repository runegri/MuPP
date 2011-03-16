
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreLocation;
using MonoTouch.MapKit;
using System.Threading;
using Core;

namespace iPhoneUI
{
	public class EmptyViewController : UIViewController
	{
		UITableView _tableView;		
		public override void ViewDidLoad ()
		{
			_tableView = new UITableView(View.Bounds, UITableViewStyle.Grouped);
			
			View.AddSubview(_tableView);
		}
	}
}

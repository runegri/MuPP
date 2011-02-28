
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
	public class MainViewController : UIViewController
	{
		UITableView _tableView;
		
		public override void ViewDidLoad ()
		{
			_tableView = new UITableView(View.Bounds, UITableViewStyle.Grouped);
			
			//_scrollView = new UIScrollView(View.Bounds);
			
			_tableView.Source = new AdvancedTableViewSource(new List<string> {"Buenget", "Churchills veg", "Dalen Hageby", "Fagervika", "Fjøslia", "Munkvoll Gård", "Prinsenskrysset"});
			
			View.AddSubview(_tableView);
		}
	}
}

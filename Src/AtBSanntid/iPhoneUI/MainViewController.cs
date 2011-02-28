
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreLocation;
using MonoTouch.MapKit;
using System.Threading;
using Holdeplasser;
namespace iPhoneUI
{
	public class MainViewController : UIViewController
	{
		UITableView _tableView;		
		public override void ViewDidLoad ()
		{
			_tableView = new UITableView(View.Bounds, UITableViewStyle.Grouped);
			
			//_scrollView = new UIScrollView(View.Bounds);
			var vm = new HoldeplasserViewModel();
			
			_tableView.Source = new AdvancedTableViewSource(vm.Stops.Select(s => s.StopName).ToList());
			View.AddSubview(_tableView);
		}
	}
}

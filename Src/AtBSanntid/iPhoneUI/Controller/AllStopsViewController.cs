
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
	public class AllStopsViewController : UIViewController
	{
		private UITableView _tableView;
		
		public override void ViewDidLoad ()
		{	
			this.Title = "Holdeplasser";
			_tableView = new UITableView(View.Bounds, UITableViewStyle.Grouped);
			
			var vm = new HoldeplasserViewModel();
			
			_tableView.Source = new BusStopTableViewSource(this, vm.Stops);
			//_tableView.Delegate = new AllStopsTableViewDelegate();
			
			View.AddSubview(_tableView);
		}
	}	
}

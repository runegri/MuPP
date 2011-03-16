
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreLocation;
using MonoTouch.MapKit;
using System.Threading;
using AtB;
namespace iPhoneUI
{
	public class AllStopsViewController : UIViewController
	{
		private IBusStopRepository _busStopRepository;
		private UITableView _tableView;
		
		public AllStopsViewController(IBusStopRepository busStopRepository)
		{
			_busStopRepository = busStopRepository;
		}
		
		public override void ViewDidLoad ()
		{	
			this.Title = "Alle";
			_tableView = new UITableView(View.Bounds, UITableViewStyle.Grouped);
			
			_tableView.Source = new BusStopTableViewSource(this, _busStopRepository.GetAll());
			
			View.AddSubview(_tableView);
		}
	}	
}

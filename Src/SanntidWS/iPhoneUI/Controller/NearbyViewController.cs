using System;
using MonoTouch.UIKit;
using System.Collections.Generic;
using System.Linq;
using AtB;

namespace iPhoneUI
{
	public class NearbyViewController : UIViewController
	{
		private IBusStopRepository _busStopRepository;
		private UITableView _tableView;		
		
		private UIBarButtonItem _mapButton;
		
		private IList<BusStop> _nearbyBusStops;
		
		public NearbyViewController(IBusStopRepository busStopRepository)
		{
			_busStopRepository = busStopRepository;			
		}
		
		public override void ViewDidLoad ()
		{	
			this.Title = "Nær deg";
			
			_mapButton = new UIBarButtonItem("Kart", UIBarButtonItemStyle.Bordered, delegate {
				NavigationController.PushViewController(new MapViewController("Nær deg", _nearbyBusStops.ToArray()), true);
			});
			
			NavigationItem.RightBarButtonItem = _mapButton;
			
			_tableView = new UITableView(View.Bounds, UITableViewStyle.Plain);
			
			View.AddSubview(_tableView);
		}
		
		public override void ViewDidAppear (bool animated)
		{
			SetNearbyBusStops();
		}
		
		private void SetNearbyBusStops()
		{
			_nearbyBusStops = _busStopRepository.GetNearby();
			
			NavigationItem.RightBarButtonItem.Enabled = _nearbyBusStops.Count > 0;
			
			_tableView.Source = new SimpleBusStopTableViewSource(this, _busStopRepository, _nearbyBusStops);
		}
	}
}


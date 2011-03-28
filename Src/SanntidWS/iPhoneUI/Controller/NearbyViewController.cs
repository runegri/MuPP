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
		public UITableView _tableView;		
		
		private UIBarButtonItem _mapButton;
		
		public IList<BusStop> _nearbyBusStops;
		
		public NearbyViewController(IBusStopRepository busStopRepository)
		{
			_busStopRepository = busStopRepository;			
		}
		
		public RefreshTableHeaderView _refreshHeaderView;
		
		public override void ViewDidLoad ()
		{	
			this.Title = "Nær deg";
			
			_mapButton = new UIBarButtonItem("Kart", UIBarButtonItemStyle.Bordered, delegate {
				NavigationController.PushViewController(new MapViewController("Nær deg", _nearbyBusStops.ToArray()), true);
			});
			
			NavigationItem.RightBarButtonItem = _mapButton;
			
			_tableView = new UITableView(View.Bounds, UITableViewStyle.Plain);
			
			View.AddSubview(_tableView);
			
			_refreshHeaderView = new RefreshTableHeaderView ();

			_tableView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;

			_tableView.AddSubview (_refreshHeaderView);
		}
		
		public override void ViewDidAppear (bool animated)
		{
			SetNearbyBusStops();
		}
		
		public void SetNearbyBusStops()
		{
			_nearbyBusStops = _busStopRepository.GetNearby();
			
			_tableView.Source = new NearbyTableViewSource(this, _busStopRepository, _nearbyBusStops);
		}
	}
}
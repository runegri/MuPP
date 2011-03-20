using System;
using MonoTouch.UIKit;
using System.Collections.Generic;
using AtB;

namespace iPhoneUI
{
	public class NearbyViewController : UIViewController
	{
		private IBusStopRepository _busStopRepository;
		private UITableView _tableView;		
		
		public NearbyViewController(IBusStopRepository busStopRepository)
		{
			_busStopRepository = busStopRepository;			
		}
		
		public override void ViewDidLoad ()
		{	
			this.Title = "Nær deg";
			_tableView = new UITableView(View.Bounds, UITableViewStyle.Plain);
						
			_tableView.Source = new SimpleBusStopTableViewSource(this, _busStopRepository, _busStopRepository.GetNearby());
			
			View.AddSubview(_tableView);
		}
		
		public override void ViewDidAppear (bool animated)
		{
			_tableView.Source = new SimpleBusStopTableViewSource(this, _busStopRepository, _busStopRepository.GetNearby());
		}
	}
	

	

}


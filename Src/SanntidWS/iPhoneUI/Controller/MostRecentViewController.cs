using System;
using MonoTouch.UIKit;
using System.Collections.Generic;
using AtB;

namespace iPhoneUI
{
	public class MostRecentViewController : UIViewController
	{
		private IBusStopRepository _busStopRepository;
		private UITableView _tableView;
		
		public MostRecentViewController(IBusStopRepository busStopRepository)
		{
			_busStopRepository = busStopRepository;
		}
		
		public override void ViewDidLoad ()
		{	
			this.Title = "Sist brukte";
			_tableView = new UITableView(View.Bounds, UITableViewStyle.Plain);
						
			_tableView.Source = new SimpleBusStopTableViewSource(this, _busStopRepository, _busStopRepository.GetMostRecent());
			
			View.AddSubview(_tableView);
		}
		
		
		public override void ViewWillAppear (bool animated)
		{
			_tableView.Source = new SimpleBusStopTableViewSource(this, _busStopRepository, _busStopRepository.GetMostRecent());			
		}
	}
}


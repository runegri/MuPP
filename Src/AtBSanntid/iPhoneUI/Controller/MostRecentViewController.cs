using System;
using MonoTouch.UIKit;
using Core;
using System.Collections.Generic;

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
		
		public MostRecentViewController() : this(new BusStopRepository())
		{
			
		}
		
		public override void ViewDidLoad ()
		{	
			this.Title = "Sist brukte";
			_tableView = new UITableView(View.Bounds, UITableViewStyle.Grouped);
						
			_tableView.Source = new BusStopTableViewSource(this, _busStopRepository.GetMostRecent());
			
			View.AddSubview(_tableView);
		}
	}
	

	

}


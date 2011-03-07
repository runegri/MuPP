using System;
using MonoTouch.UIKit;
using Core;
using System.Collections.Generic;

namespace iPhoneUI
{
	public class FavoritesViewController : UIViewController
	{
		private IBusStopRepository _busStopRepository;
		private UITableView _tableView;
		
		public FavoritesViewController(IBusStopRepository busStopRepository)
		{
			_busStopRepository = busStopRepository;
		}
		
		public FavoritesViewController() : this(new BusStopRepository())
		{
			
		}
		
		public override void ViewDidLoad ()
		{	
			this.Title = "Favoritter";
			_tableView = new UITableView(View.Bounds, UITableViewStyle.Grouped);
						
			_tableView.Source = new BusStopTableViewSource(this, _busStopRepository.GetFavorites());
			//_tableView.Delegate = new AllStopsTableViewDelegate();
			
			View.AddSubview(_tableView);
		}
	}
	

	

}


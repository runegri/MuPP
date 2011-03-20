using System;
using MonoTouch.UIKit;
using System.Collections.Generic;
using AtB;

namespace iPhoneUI
{
	public class FavoritesViewController : UIViewController
	{
		private IBusStopRepository _busStopRepository;
		private UITableView _tableView;
		
		private UIBarButtonItem _editButton;
		private UIBarButtonItem _doneButton;
		
		public FavoritesViewController(IBusStopRepository busStopRepository)
		{
			_busStopRepository = busStopRepository;
			
			_editButton = new UIBarButtonItem(UIBarButtonSystemItem.Edit);
			_doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done);
			
			NavigationItem.RightBarButtonItem = _editButton;
			
			_editButton.Clicked += delegate {
				_tableView.Editing = true;
				Editing = true;
				NavigationItem.RightBarButtonItem = _doneButton;
			};
			
			_doneButton.Clicked += delegate {
				_tableView.Editing = false;
				Editing = false;
				NavigationItem.RightBarButtonItem = _editButton;
			};
		}
		
		public override void ViewDidLoad ()
		{	
			this.Title = "Favoritter";
			_tableView = new UITableView(View.Bounds, UITableViewStyle.Plain);
						
			_tableView.Source = new SimpleBusStopTableViewSource(this, _busStopRepository, _busStopRepository.GetFavorites());
			
			View.AddSubview(_tableView);
		}
		
		public override void ViewDidAppear (bool animated)
		{
			_tableView.Source = new SimpleBusStopTableViewSource(this, _busStopRepository, _busStopRepository.GetFavorites());
		}
		
		

	}
	

	

}


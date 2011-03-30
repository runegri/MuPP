
using System.Collections.Generic;
using AtB;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System;
namespace iPhoneUI
{
	public class NearbyTableViewSource : SimpleBusStopTableViewSource
	{
		bool _checkForRefresh = false;
		bool _reloading = false;
		NSTimer _reloadTimer;
		RefreshTableHeaderView _refreshHeaderView;
		UITableView _tableView;
		
		NearbyViewController _nearbyViewController;

		public NearbyTableViewSource(NearbyViewController nearByViewController,IBusStopRepository busStopRepository,  
                 IList<BusStop> busStops) : base(nearByViewController, busStopRepository, busStops)
		{
			_refreshHeaderView = nearByViewController._refreshHeaderView;
			_tableView = nearByViewController._tableView;
			_nearbyViewController = nearByViewController;
		}
		
		[Export("scrollViewDidScroll:")]
		public new void Scrolled (UIScrollView scrollView)
		{
			float contentOffsetY = _tableView.ContentOffset.Y;
			
			if(!_checkForRefresh) return;
			
			if (_refreshHeaderView._isFlipped && (contentOffsetY > -65f) && (contentOffsetY < 0f) && !_reloading) {
				//_refreshHeaderView.FlipImageAnimated (true);
				_refreshHeaderView.SetPullToUpdateStatus();
			} else if ((!_refreshHeaderView._isFlipped) && (contentOffsetY < -65f)) {
				//_refreshHeaderView.FlipImageAnimated (true);
				_refreshHeaderView.SetReleaseToUpdateStatus();
			}
		}
		
		[Export("scrollViewWillBeginDragging:")]
		public new void DraggingStarted (UIScrollView scrollView)
		{
			_checkForRefresh = true;
		}

		[Export("scrollViewDidEndDragging:willDecelerate:")]
		public new void DraggingEnded (UIScrollView scrollView, bool willDecelerate)
		{
			if (_tableView.ContentOffset.Y <= -65f) {
				

				_reloading = true;
				_tableView.ReloadData ();
				_refreshHeaderView.ToggleActivityView ();
				UIView.BeginAnimations ("ReloadingData");
				UIView.SetAnimationDuration (0.2);
				_tableView.ContentInset = new UIEdgeInsets (60f, 0f, 0f, 0f);
				UIView.CommitAnimations ();
				
				_nearbyViewController.SetNearbyBusStops();
				
				//ReloadTimer = NSTimer.CreateRepeatingScheduledTimer (new TimeSpan (0, 0, 0, 10, 0), () => dataSourceDidFinishLoadingNewData ());
				//_reloadTimer = NSTimer.CreateScheduledTimer (TimeSpan.FromSeconds (2f), delegate {
					_reloadTimer = null;
					_reloading = false;
					//_refreshHeaderView.FlipImageAnimated (false);
					_refreshHeaderView.ToggleActivityView ();
					UIView.BeginAnimations ("DoneReloadingData");
					UIView.SetAnimationDuration (0.3);
					_tableView.ContentInset = new UIEdgeInsets (0f, 0f, 0f, 0f);
					_refreshHeaderView.SetPullToUpdateStatus();
					UIView.CommitAnimations ();
					_refreshHeaderView.SetCurrentDate ();
				//});

			}

			_checkForRefresh = false;
		}
		
	}
}

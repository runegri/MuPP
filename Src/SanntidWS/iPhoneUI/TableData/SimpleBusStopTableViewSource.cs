using System.Collections.Generic;
using AtB;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace iPhoneUI
{
	public class SimpleBusStopTableViewSource : BaseBusStopTableViewSource
	{
		private IList<BusStop> _busStops;
				
		public SimpleBusStopTableViewSource (UIViewController controller,  IList<BusStop> busStops)
		{
			_controller = controller;
			
			_busStops = busStops;
		}

		protected override BusStop GetBusStop(NSIndexPath indexPath)
		{
			return _busStops[indexPath.Row];
		}
		
		public override int RowsInSection (UITableView tableview, int section)
		{
			return _busStops.Count;
		}
		
		public override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
		{
			if(editingStyle == UITableViewCellEditingStyle.Delete)
			{
				_busStops.Remove(GetBusStop(indexPath));
				tableView.DeleteRows(new [] {indexPath}, UITableViewRowAnimation.Fade);
			}
		}
	}
}
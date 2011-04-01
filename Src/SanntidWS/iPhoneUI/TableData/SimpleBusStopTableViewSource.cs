using System.Collections.Generic;
using AtB;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System;

namespace iPhoneUI
{
	public class SimpleBusStopTableViewSource : BaseBusStopTableViewSource
	{
		protected IList<BusStop> _busStops;
		protected IBusStopRepository _busStopRepository;
		
		public SimpleBusStopTableViewSource (
                 UIViewController controller, 
                 IBusStopRepository busStopRepository,  
                 IList<BusStop> busStops)
		{
			_controller = controller;
			_busStopRepository = busStopRepository;
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
				var busStop = GetBusStop(indexPath);
				_busStops.Remove(busStop);
				_busStopRepository.RemoveFavorite(busStop);
				tableView.DeleteRows(new [] {indexPath}, UITableViewRowAnimation.Fade);
			}
		}
	}
	
	

}

using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreLocation;
using MonoTouch.MapKit;
using System.Threading;
using System.Drawing; 
using System.Text;
using AtB;

namespace iPhoneUI
{
	public class SimpleBusStopTableViewSource : UITableViewSource
	{
		private IList<BusStop> _busStops;
		
		private UIViewController _controller;
		
		public SimpleBusStopTableViewSource (UIViewController controller,  IList<BusStop> busStops)
		{
			_controller = controller;
			
			_busStops = busStops;
		}

		private BusStop GetBusStop(NSIndexPath indexPath)
		{
			return _busStops[indexPath.Row];
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			
			UITableViewCell cell = CreateOrReuseCell(tableView);
			
			BusStop stopInfo = GetBusStop(indexPath);
			cell.TextLabel.Text = stopInfo.Name;
			
			return cell;
		}
		
		public override int RowsInSection (UITableView tableview, int section)
		{
			return _busStops.Count;
		}
		
		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			BusStop stopInfo = GetBusStop(indexPath);
			var uivc = new BusStopViewController(stopInfo);
			
			_controller.NavigationController.PushViewController (uivc, true);
			
			// Prevent the blue 'selection indicator' remaining.
			tableView.DeselectRow (indexPath, true);
		}
		
		public override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
		{
			if(editingStyle == UITableViewCellEditingStyle.Delete)
			{
				_busStops.Remove(GetBusStop(indexPath));
				tableView.DeleteRows(new [] {indexPath}, UITableViewRowAnimation.Fade);
			}
		}
		
		private static UITableViewCell CreateOrReuseCell(UITableView tableView)
		{
			string kCellIdentifier = "mycell";
			UITableViewCell cell = tableView.DequeueReusableCell(kCellIdentifier);			

			if (cell != null) return cell;
			
			cell = new UITableViewCell (UITableViewCellStyle.Default, kCellIdentifier);
			cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
			return cell;
		}
}


}
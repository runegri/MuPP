
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
	public abstract class BaseBusStopTableViewSource : UITableViewSource
	{
		protected UIViewController _controller;
		
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			
			UITableViewCell cell = CreateOrReuseCell(tableView);
			
			BusStop stopInfo = GetBusStop(indexPath);
			cell.TextLabel.Text = stopInfo.Name;
			cell.DetailTextLabel.Text = stopInfo.TowardsCentre ? "Til sentrum" : "Fra sentrum";
			
			return cell;
		}
		
		private static UITableViewCell CreateOrReuseCell(UITableView tableView)
		{
			string kCellIdentifier = "mycell";
			UITableViewCell cell = tableView.DequeueReusableCell(kCellIdentifier);			

			if (cell != null) return cell;
			
			cell = new UITableViewCell (UITableViewCellStyle.Subtitle, kCellIdentifier);
			cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
			return cell;
		}
		
		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			BusStop stopInfo = GetBusStop(indexPath);
			var uivc = new BusStopViewController(stopInfo);
			
			_controller.NavigationController.PushViewController (uivc, true);
			
			// Prevent the blue 'selection indicator' remaining.
			tableView.DeselectRow (indexPath, true);
		}
		
		protected abstract BusStop GetBusStop(NSIndexPath indexPath);
	}	
}

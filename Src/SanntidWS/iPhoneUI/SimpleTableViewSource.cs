
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreLocation;
using MonoTouch.MapKit;
using System.Threading;
namespace iPhoneUI
{
	public class SimpleTableViewSource : UITableViewSource
	{
		private List<string> rows;
		public SimpleTableViewSource (List<string> list)
		{
			rows = list;
		}
		
		public override int RowsInSection (UITableView tableview, int section)
		{
			return rows.Count;
		} 
	
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = new UITableViewCell(UITableViewCellStyle.Default, "mycell");
			cell.TextLabel.Text = rows[indexPath.Row];
			return cell;
			
		}		
	}
}

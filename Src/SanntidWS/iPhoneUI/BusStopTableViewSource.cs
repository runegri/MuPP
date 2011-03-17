
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
	public class BusStopTableViewSource : UITableViewSource
	{
		private List<string> _sectionTitles;
		private SortedDictionary<int, List<BusStop>> _sectionElements = new SortedDictionary<int, List<BusStop>> ();
		
		private UIViewController _controller;
		
		
		
		public BusStopTableViewSource (UIViewController controller,  IEnumerable<BusStop> busStops)
		{
			_controller = controller;
			
			_sectionTitles = (from c in busStops
				select c.Name.Substring (0, 1)).Distinct ().ToList ();
			
			_sectionTitles.Sort();
			
			foreach (BusStop stopInfo in busStops) {
				int sectionNum = _sectionTitles.IndexOf (stopInfo.Name.Substring (0, 1));
				if (_sectionElements.ContainsKey (sectionNum)) {
					_sectionElements[sectionNum].Add (stopInfo);
				} else {
					_sectionElements.Add (sectionNum, new List<BusStop> { stopInfo });
				}
			}
		}

		public override int NumberOfSections (UITableView tableView)
		{
			return _sectionTitles.Count;
		}

		public override string TitleForHeader (UITableView tableView, int section)
		{
			return _sectionTitles[section];
		}

		public override string[] SectionIndexTitles (UITableView tableView)
		{
			return _sectionTitles.ToArray ();
		}

		public override int RowsInSection (UITableView tableview, int section)
		{
			return _sectionElements[section].Count;
		}

		private BusStop GetBusStop(NSIndexPath indexPath)
		{
			return _sectionElements[indexPath.Section][indexPath.Row];
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			
			UITableViewCell cell = CreateOrReuseCell(tableView);
			
			BusStop stopInfo = GetBusStop(indexPath);
			cell.TextLabel.Text = stopInfo.Name;
			cell.DetailTextLabel.Text = stopInfo.TowardsCentre ? "Til sentrum" : "Fra sentrum";
			
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
		
		public override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
		{
			if(editingStyle == UITableViewCellEditingStyle.Delete)
			{
				_sectionElements[indexPath.Section].Remove(GetBusStop(indexPath));
				tableView.DeleteRows(new [] {indexPath}, UITableViewRowAnimation.Fade);
			}
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
}


}
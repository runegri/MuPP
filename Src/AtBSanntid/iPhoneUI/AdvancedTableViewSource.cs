
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
	public class AdvancedTableViewSource : UITableViewSource
	{
		private List<string> _sectionTitles;
		private SortedDictionary<int, List<string>> _sectionElements = new SortedDictionary<int, List<string>> ();

		public AdvancedTableViewSource (List<string> list)
		{
			_sectionTitles = (from c in list
				select c.Substring (0, 1)).Distinct ().ToList ();
			_sectionTitles.Sort ();
			
			foreach (string element in list) {
				int sectionNum = _sectionTitles.IndexOf (element.Substring (0, 1));
				if (_sectionElements.ContainsKey (sectionNum)) {
					_sectionElements[sectionNum].Add (element);
				} else {
					_sectionElements.Add (sectionNum, new List<string> { element });
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

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			string kCellIdentifier = "mycell";
			UITableViewCell cell = tableView.DequeueReusableCell (kCellIdentifier);
			if (cell == null) {
				// No re-usable cell found, create a new one.
				cell = new UITableViewCell (UITableViewCellStyle.Default, kCellIdentifier);
			}
			
			string display = _sectionElements[indexPath.Section][indexPath.Row];
			cell.TextLabel.Text = display;
			
			return cell;
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			string display = _sectionElements[indexPath.Section][indexPath.Row];
			
			showAlert ("RowSelected", "You selected: \"" + display + "\"");
			
			// Prevent the blue 'selection indicator' remaining.
			tableView.DeselectRow (indexPath, true);
		}

		private void showAlert (string title, string message)
		{
			using (var alert = new UIAlertView (title, message, null, "OK", null)) {
				alert.Show ();
			}
		}
	}
}

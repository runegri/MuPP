
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
	public class BusStopTableViewSource : BaseBusStopTableViewSource
	{
		private List<string> _sectionTitles;
		private SortedDictionary<int, List<BusStop>> _sectionElements = new SortedDictionary<int, List<BusStop>> ();
		
		
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

		protected override BusStop GetBusStop(NSIndexPath indexPath)
		{
			return _sectionElements[indexPath.Section][indexPath.Row];
		}		
	}
}
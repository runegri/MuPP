
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreLocation;
using MonoTouch.MapKit;
using System.Threading;
using AtB;
using System.Drawing;
namespace iPhoneUI
{
	public class AllStopsViewController : UIViewController
	{
		private IBusStopRepository _busStopRepository;
		private UITableView _tableView;
		
		public AllStopsViewController(IBusStopRepository busStopRepository)
		{
			_busStopRepository = busStopRepository;
		}
		
		public override void ViewDidLoad ()
		{	
			this.Title = "Alle";
			
			// Hack! Not sure if 90 pixels are correct on all models
			RectangleF rec = new RectangleF(View.Bounds.X, View.Bounds.Y, View.Bounds.Width, View.Bounds.Height - 90);
			
			_tableView = new UITableView(rec, UITableViewStyle.Grouped);
			
			_tableView.Source = new BusStopTableViewSource(this, _busStopRepository.GetAll());
			
			View.AddSubview(_tableView);
		}
	}	
}

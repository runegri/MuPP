using System.Collections.Generic;
using System.Linq;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using System.Drawing;
using System;
using AtB;

namespace iPhoneUI
{
	public class BusStopViewController : UIViewController
	{
		private UIView _busStopView = new UIView();
		private UIView _realTimeDataView = new UIView();
		
		private BusStop _busStop;
		private IBusStopRepository _busStopRepository = TinyIoC.TinyIoCContainer.Current.Resolve<IBusStopRepository> ();
		
		private UIActivityIndicatorView _activityIndicator;
		
		private UILabel _statusLabel;
		
		private string ClockSymbol = char.ConvertFromUtf32(0x231A);

		const int xMargin = 10;
		const int width = 300;
		
		public BusStopViewController (BusStop stopInfo)
		{
			_busStop = stopInfo;
			_busStopRepository.AddMostRecent (_busStop);
			this.Title = _busStop.Name;
		}

		public override void ViewDidLoad ()
		{
			_busStopView.BackgroundColor = UIColor.GroupTableViewBackgroundColor;
						
			int yPos = 5;
			
			UILabel viewTitle = LabelFactory.CreateViewTitleLabel(_busStop.Name, xMargin, yPos, width);
			_busStopView.Add (viewTitle);
			
			yPos+= 22;
			
			UILabel subTitleLabel = LabelFactory.CreateSubTitleLabel(_busStop.TowardsCentreText + " (" + _busStop.ShortStopCode + ")", xMargin, yPos, 300);
			_busStopView.Add (subTitleLabel);
			
			yPos += 35;
			
			UIButton favoritesButton = ButtonFactory.CreateButton("Legg til i\nfavoritter", xMargin, yPos);
			favoritesButton.TouchUpInside += delegate { _busStopRepository.AddFavorite (_busStop); };			
			_busStopView.Add (favoritesButton);
			
			_busStopView.SizeToFit ();			
			
			UIButton mapButton = ButtonFactory.CreateButton("Se kart", 165, yPos);
			mapButton.TouchUpInside += delegate { 
				NavigationController.PushViewController (new MapViewController(_busStop.Name, _busStop), true);
			};
			_busStopView.Add (mapButton);
			
			yPos += 65;
			
			_realTimeDataView.Layer.CornerRadius = 5;
			_realTimeDataView.Layer.BorderColor = UIColor.LightGray.CGColor;
			_realTimeDataView.Layer.BorderWidth = 1;
			_realTimeDataView.Frame = new RectangleF(xMargin, yPos, width, 175);
			_realTimeDataView.BackgroundColor = UIColor.White;
			_busStopView.Add(_realTimeDataView);
			
			yPos+= 175;
			
			_statusLabel = LabelFactory.CreateSubTitleLabel("", xMargin, yPos , width);
			_statusLabel.TextAlignment = UITextAlignment.Center;
			_busStopView.Add(_statusLabel);
			
			_activityIndicator = new UIActivityIndicatorView();
			_activityIndicator.Frame = new RectangleF(137, 68, 30, 30);
			
			_activityIndicator.ActivityIndicatorViewStyle = UIActivityIndicatorViewStyle.Gray;
			
			_realTimeDataView.Add(_activityIndicator);
			
			_busStopView.Frame = new RectangleF (0, 0, this.View.Bounds.Width, this.View.Bounds.Height);
			
			
			
			this.View.AddSubview (_busStopView);
			
			
			
			var refreshButton = new UIBarButtonItem("Oppdater", UIBarButtonItemStyle.Bordered, delegate { RefreshRealTimeData(); });
			NavigationItem.RightBarButtonItem = refreshButton;
			RefreshRealTimeData();
		}
		
		private void RefreshRealTimeData()
		{
			_statusLabel.Text = "";
			
			foreach(var subView in _realTimeDataView.Subviews) 
			{
				subView.RemoveFromSuperview();
			}
			
			_realTimeDataView.Add(_activityIndicator);
			_activityIndicator.StartAnimating();
			
			_busStopRepository.GetRealTimeData (_busStop, RealTimeDataLoaded);
		}
		
		private void RealTimeDataLoaded (List<StopTime> stops)
		{
			InvokeOnMainThread (() =>
			{
				_activityIndicator.StopAnimating();

				var yPos = 12;
				if (stops.Any ()) 
				{
					foreach (var stop in stops)
					{
						
						UILabel routeLabel = LabelFactory.CreateRegularLabel("Rute " + stop.RouteNr, 10, yPos, 80);
						_realTimeDataView.Add (routeLabel);
						
						UILabel timeLabel = LabelFactory.CreateRegularLabel(stop.GetTime(), 100, yPos, 65);
						timeLabel.TextAlignment = UITextAlignment.Right;
						_realTimeDataView.Add (timeLabel);
						
						if(stop.IsRealTime())
						{
							UILabel realTimeIndicatorLabel= LabelFactory.CreateRegularLabel(ClockSymbol, 170, yPos, 10); 
							_realTimeDataView.Add (realTimeIndicatorLabel);
						}
						
						UILabel timeRemaining = LabelFactory.CreateRegularLabel(stop.GetTimeDifference(), 200, yPos, 80);
						timeRemaining.TextAlignment = UITextAlignment.Center;
						_realTimeDataView.Add (timeRemaining);
						
						yPos += 30;
					}
				} 
				else 
				{
					UILabel label = LabelFactory.CreateRegularLabel("Fant ingen data for holdeplassen", 25, 65, width);
					_realTimeDataView.Add (label);
				}
				
				_statusLabel.Text = "oppdatert " + DateTime.Now.ToString("HH:mm:ss");
			});
		}
	}
}
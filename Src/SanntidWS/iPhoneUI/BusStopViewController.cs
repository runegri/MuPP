using System.Collections.Generic;
using System.Linq;
using MonoTouch.UIKit;
using System.Drawing;
using System;
using AtB;

namespace iPhoneUI
{
	public class BusStopViewController : UIViewController
	{
		private UIView _view;
		private BusStop _busStop;
		private IBusStopRepository _busStopRepository = TinyIoC.TinyIoCContainer.Current.Resolve<IBusStopRepository> ();
		private UIActivityIndicatorView _activityIndicator;
		
		private List<UILabel> _realTimeData = new List<UILabel>();
		
		public BusStopViewController (BusStop stopInfo)
		{
			_busStop = stopInfo;
			_busStopRepository.AddMostRecent (_busStop);
			this.Title = _busStop.Name;
		}

		public override void ViewDidLoad ()
		{
			_view = new UIView ();
			_view.BackgroundColor = UIColor.GroupTableViewBackgroundColor;
						
			int y = 5;
			int x = 10;
			
			UILabel label = new UILabel();
			label.Font = UIFont.BoldSystemFontOfSize(20);
			label.Text = _busStop.Name;
			label.BackgroundColor = UIColor.Clear;
			label.ShadowOffset = new SizeF(0,1);
			label.ShadowColor = UIColor.White;
			label.Frame = new RectangleF (x, y, 300, 30);
			
			_view.Add (label);
			
			y+= 22;
			
			UILabel label2 = new UILabel();
			label2.Font = UIFont.SystemFontOfSize(14);
			label2.TextColor = UIColor.DarkGray;
			label2.Text = _busStop.TowardsCentreText;
			label2.BackgroundColor = UIColor.Clear;
			label2.Frame = new RectangleF (x, y, 300, 30);
			
			_view.Add (label2);
			
			y += 35;
			
			UIButton favoritesButton = CreateButton("Legg til i\nfavoritter", x, y);
			favoritesButton.TouchUpInside += delegate { _busStopRepository.AddFavorite (_busStop); };			
			_view.Add (favoritesButton);
			
			_view.SizeToFit ();			
			
			UIButton mapButton = CreateButton("Se kart", 165, y);
			mapButton.TouchUpInside += delegate { 
				NavigationController.PushViewController (new MapViewController(_busStop.Name, _busStop), true);
			};
			_view.Add (mapButton);
			
			_activityIndicator = new UIActivityIndicatorView();
			_activityIndicator.Frame = new RectangleF(View.Frame.Width - 50, 10, 30, 30);
			_activityIndicator.ActivityIndicatorViewStyle = UIActivityIndicatorViewStyle.Gray;
			
			_view.Add(_activityIndicator);
						
			_view.Frame = new RectangleF (0, 0, this.View.Bounds.Width, this.View.Bounds.Height);
			
			this.View.AddSubview (_view);
			
			var refreshButton = new UIBarButtonItem("Oppdater", UIBarButtonItemStyle.Bordered, delegate { RefreshRealTimeData(); });
			NavigationItem.RightBarButtonItem = refreshButton;
			RefreshRealTimeData();
		}
		
		private void RefreshRealTimeData()
		{
			_activityIndicator.StartAnimating();
			_busStopRepository.GetRealTimeData (_busStop, RealTimeDataLoaded);
		}
		
		private UIButton CreateButton(string title, int xPos, int yPos)
		{
			UIButton button = UIButton.FromType (UIButtonType.RoundedRect);
			button.TitleLabel.LineBreakMode = UILineBreakMode.WordWrap;
			button.TitleLabel.TextAlignment = UITextAlignment.Center;			
			button.SetTitle (title, UIControlState.Normal);		
			button.Frame = new RectangleF (xPos, yPos, 145, 50);
			return button;
		}
		
		private void RealTimeDataLoaded (List<StopTime> stops)
		{
			
			InvokeOnMainThread (() =>
			{
				foreach(var label in _realTimeData)
				{
					label.RemoveFromSuperview();
				}
				_activityIndicator.StopAnimating();
				var y = 140;
				if (stops.Any ()) 
				{
					foreach (var stop in stops)
					{
						UILabel label = new UILabel ();
						label.Text = stop.ToString ();
						label.BackgroundColor = UIColor.Clear;
						label.Frame = new RectangleF (10, y, 300, 30);
						_view.Add (label);
						y += 30;
						_realTimeData.Add(label);
					}
				} 
				else 
				{
					UILabel label = new UILabel ();
					label.Text = "Fant ingen data for holdeplassen";
					label.BackgroundColor = UIColor.Clear;
					label.Frame = new RectangleF (10, y, 300, 30);
					_view.Add (label);
					y += 30;
					_realTimeData.Add(label);
				}
				
				
				UILabel label2 = new UILabel();
				label2.Font = UIFont.SystemFontOfSize(14);
				label2.TextColor = UIColor.DarkGray;
				label2.Text = "oppdatert " + DateTime.Now.ToString("HH:mm:ss");
				label2.BackgroundColor = UIColor.Clear;
				label2.Frame = new RectangleF (10, y, 300, 30);
				_view.Add (label2);
				_realTimeData.Add(label2);
			});
		}
	}
}
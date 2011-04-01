using System;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.CoreAnimation;
using MonoTouch.CoreGraphics;

namespace iPhoneUI
{
	public class RefreshTableHeaderView : UIView
	{
		private UILabel _lastUpdatedLabel;
		private UILabel _statusLabel;
		private UIActivityIndicatorView _activityView;

		public bool _isFlipped;

		public RefreshTableHeaderView () : base()
		{
			_lastUpdatedLabel = new UILabel (new RectangleF (0f, this.Frame.Height - 30f, 320f, 20f));
			_lastUpdatedLabel.Font = UIFont.SystemFontOfSize (12f);
			_lastUpdatedLabel.TextColor = UIColor.Black;
			_lastUpdatedLabel.ShadowColor = UIColor.FromWhiteAlpha (0.9f, 1f);
			_lastUpdatedLabel.ShadowOffset = new SizeF (0f, 1f);
			_lastUpdatedLabel.BackgroundColor = UIColor.Clear;
			_lastUpdatedLabel.TextAlignment = UITextAlignment.Center;

			if (NSUserDefaults.StandardUserDefaults["EGORefreshTableView_LastRefresh"] != null) {
				_lastUpdatedLabel.Text = NSUserDefaults.StandardUserDefaults["EGORefreshTableView_LastRefresh"].ToString ();
			} else {
				SetCurrentDate ();
			}

			this.AddSubview (_lastUpdatedLabel);

			_statusLabel = new UILabel (new RectangleF (0f, this.Frame.Height - 48f, 320f, 20f));
			_statusLabel.Font = UIFont.BoldSystemFontOfSize (13f);
			_statusLabel.TextColor = UIColor.Black;
			_statusLabel.ShadowColor = UIColor.FromWhiteAlpha (0.9f, 1f);
			_statusLabel.ShadowOffset = new SizeF (0f, 1f);
			_statusLabel.BackgroundColor = UIColor.Clear;
			_statusLabel.TextAlignment = UITextAlignment.Center;
			SetPullToUpdateStatus();
			this.AddSubview (_statusLabel);

			_activityView = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.Gray);
			_activityView.Frame = new RectangleF (25f, this.Frame.Height - 38f, 20f, 20f);
			_activityView.HidesWhenStopped = true;

			this.AddSubview (_activityView);

			this._isFlipped = false;
		}
		
		public void SetUpdatingStatus()
		{
			_statusLabel.Text = "Opptaterer...";
		}
		
		public void SetPullToUpdateStatus()
		{
			_statusLabel.Text = "Dra ned for å oppdatere...";
		}
			
		public void SetReleaseToUpdateStatus() 
		{
			_statusLabel.Text = "Slipp for å oppdatere...";
		}

		public void ToggleActivityView ()
		{
			if (_activityView.IsAnimating) {
				_activityView.StopAnimating ();
			} else {
				_activityView.StartAnimating ();
				SetUpdatingStatus();
			}
		}

		public void SetCurrentDate ()
		{
			_lastUpdatedLabel.Text = String.Format ("Sist oppdatert: {0}", DateTime.Now.ToString());
			NSUserDefaults.StandardUserDefaults["EGORefreshTableView_LastRefresh"] = new NSString (DateTime.Now.ToString ());
			NSUserDefaults.StandardUserDefaults.Synchronize ();
		}
	}
}

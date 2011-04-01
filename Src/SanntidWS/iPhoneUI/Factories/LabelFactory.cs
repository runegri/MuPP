
using System.Collections.Generic;
using System.Linq;
using MonoTouch.UIKit;
using System.Drawing;
using System;
using AtB;
namespace iPhoneUI
{
	public static class LabelFactory
	{
		public static UILabel CreateSubTitleLabel(string text, int xPos, int yPos, int width)
		{
			UILabel label = new UILabel();
			label.Font = UIFont.SystemFontOfSize(14);
			label.TextColor = UIColor.DarkGray;
			label.BackgroundColor = UIColor.Clear;
			label.Frame = new RectangleF (xPos, yPos, width, 30);	
			label.Text = text;
			return label;
		}
		
		public static UILabel CreateRegularLabel(string text, int xPos, int yPos, int width)
		{
			UILabel label = new UILabel ();
			label.BackgroundColor = UIColor.Clear;
			label.Frame = new RectangleF (xPos, yPos, width, 30);
			label.Text = text;
			return label;
			
		}
		
		public static UILabel CreateViewTitleLabel(string text, int xPos, int yPos, int width)
		{
			UILabel label = new UILabel();
			label.Font = UIFont.BoldSystemFontOfSize(20);
			label.Text = text;
			label.BackgroundColor = UIColor.Clear;
			label.ShadowOffset = new SizeF(0,1);
			label.ShadowColor = UIColor.White;
			label.Frame = new RectangleF (xPos, yPos, width, 30);
			
			return label;
		}
			
	}
}

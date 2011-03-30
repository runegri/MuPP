
using System.Collections.Generic;
using System.Linq;
using MonoTouch.UIKit;
using System.Drawing;
using System;
using AtB;
namespace iPhoneUI
{
	public static class ButtonFactory
	{
		public static UIButton CreateButton(string title, int xPos, int yPos)
		{
			UIButton button = UIButton.FromType (UIButtonType.RoundedRect);
			button.TitleLabel.LineBreakMode = UILineBreakMode.WordWrap;
			button.TitleLabel.TextAlignment = UITextAlignment.Center;			
			button.SetTitle (title, UIControlState.Normal);		
			button.Frame = new RectangleF (xPos, yPos, 145, 50);
			return button;
		}
	}
}

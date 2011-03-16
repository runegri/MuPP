/*
 * Copyright (c) 2010 Simon Guindon
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
 * documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
 * the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
 * and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE 
 * WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */ 

using System;
using System.Drawing;
using MonoTouch.CoreGraphics;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

using MonoTouch.Extensions.Controls;

namespace DataBoundUITableViewSource
{
	public class CustomCell : TableViewCellBase
	{
		private static UIFont font = null;
		private static UIImage background = null;
		private static UIColor textColor = null;
		
        static CustomCell()
        {
            if (font == null)
                font = UIFont.BoldSystemFontOfSize(UIFont.SystemFontSize);
			if (background == null)
				background = UIImage.FromFile(@"Images/cell-background.png");
			if (textColor == null)
				textColor = UIColor.FromRGBA(255, 255, 255, 32);
        }
		
		public CustomCell() : base(string.Empty)
		{
		}		
		
		public override void DrawContentView(System.Drawing.RectangleF rect)
		{
			DataItem item = this.DataContext as DataItem;
			
			CGContext context = UIGraphics.GetCurrentContext();
			
			// Draw background.
			background.Draw(rect);            

            context.SetFillColorWithColor(textColor.CGColor);
            this.DrawString(item.Text, new RectangleF(0, 0, 320, item.Height), font);
		}
		
		public static float GetCellHeight(DataItem item)
        {
            SizeF size = GetStringSize(item.Text, font, new SizeF(320.0f, 1000.0f), UILineBreakMode.WordWrap);
            return size.Height;
        }

	}
}


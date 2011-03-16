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
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace MonoTouch.Extensions.Controls
{
    public abstract class TableViewCellBase : UITableViewCell, IUITableViewCellContext
    {
        private static UIView sizeHelperView = new UIView();
        private TableViewCellContent _contentView;
        private Object dataContext;

        public TableViewCellBase (string reuseIdentifier) : base(UITableViewCellStyle.Default, reuseIdentifier)
        {
            _contentView = new TableViewCellContent ();
            this.Opaque = true;
            this.AddSubview (_contentView);
        }

        public override RectangleF Frame {
            get { return base.Frame; }
            set {
                var obj = this;

                base.Frame = value;

                if (_contentView != null) {
                    RectangleF boundingRectangle = this.Bounds;
                    _contentView.Frame = boundingRectangle;
                }
                
            }
        }

        public override void SetNeedsDisplay ()
        {
            base.SetNeedsDisplay ();
            if (_contentView != null)
                _contentView.SetNeedsDisplay ();
        }

        public abstract void DrawContentView (RectangleF rect);

        public object DataContext
        {
            get { return dataContext; }
            set { dataContext = value; }
        }

        public static SizeF GetStringSize(string text, UIFont font, SizeF size, UILineBreakMode lineBreakMode)
        {
            return sizeHelperView.StringSize(text, font, size, lineBreakMode);
        }

        class TableViewCellContent : UIView
        {
            public TableViewCellContent () : base(RectangleF.Empty)
            {
            }

            public override void Draw (RectangleF rect)
            {
                ((TableViewCellBase)this.Superview).DrawContentView (rect);
            }
        }
        
    }
}

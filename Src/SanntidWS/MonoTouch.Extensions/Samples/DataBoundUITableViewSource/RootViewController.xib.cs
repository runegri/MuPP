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
using System.Collections.Generic;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

using MonoTouch.Extensions.Controls;

namespace DataBoundUITableViewSource
{
	partial class RootViewController : UIViewController
	{
		private static Random random = new Random(1);		
		private DataBoundUITableViewSource<CustomCell, DataItem> dataSource = null;
		
		public RootViewController (IntPtr handle) : base(handle)
		{
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad();
			//Show an edit button
			//NavigationItem.RightBarButtonItem = EditButtonItem;	
			
			this.Title = "Sample";
			
			if (tableView != null)
			{
				dataSource = new DataBoundUITableViewSource<CustomCell, DataItem>(tableView);
				dataSource.RowAnimation = UITableViewRowAnimation.None;
				dataSource.RowSelect += HandleDataSourceRowSelect;
				dataSource.RowDeselect += HandleDataSourceRowDeselect;
				
				int count = 500;
				for (int i=0; i<count; i++)
				{
					DataItem item = new DataItem();
					
					int rand = random.Next(28, 144);					
					for (int c=1; c<rand; c++)
					{
						item.Text += "Test " + c.ToString() + " ";
					}
					
					item.Height = CustomCell.GetCellHeight(item);
					dataSource.List.Add(item);
				}							
				
				tableView.Source = dataSource;
			}
		}

		void HandleDataSourceRowSelect (object sender, DataItem item)
		{
			Console.WriteLine(item.Height);
		}

		void HandleDataSourceRowDeselect (object sender, DataItem item)
		{
			Console.WriteLine(item.Height);
		}

		/*
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear(animated);
		}
		*/
		/*
		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear(animated);
		}
		*/
		/*
		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		}
		*/
		/*
		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
		}
		*/

		/*
		// Override to allow orientations other than the default portrait orientation
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			//return true for supported orientations
			return (InterfaceOrientation == UIInterfaceOrientation.Portrait);
		}
		*/

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidUnload ()
		{
			// Release anything that can be recreated in viewDidLoad or on demand.
			// e.g. this.myOutlet = null;
			
			base.ViewDidUnload ();
		}				
	}
	
}


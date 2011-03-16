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
using System.Collections.ObjectModel;
using System.Collections.Specialized;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace MonoTouch.Extensions.Controls
{
    public delegate void RowSelectHandler<TModel>(object sender, TModel item);

    public class DataBoundUITableViewSource<TCell, TModel> : UITableViewSource
    {
        public event RowSelectHandler<TModel> RowSelect;
        public event RowSelectHandler<TModel> RowDeselect;

        private UITableView tableView = null;
        private ObservableCollection<TModel> list = null;

        // --------------------------------------------------
        // Constructors
        // --------------------------------------------------
        #region Constructors
        public DataBoundUITableViewSource(UITableView tableView)
        {
            this.tableView = tableView;
            list = new ObservableCollection<TModel>();
            Initialize();
        }

        public DataBoundUITableViewSource(UITableView tableView, ObservableCollection<TModel> list)
        {
            this.tableView = tableView;
            this.list = list;
            Initialize();
        }

        private void Initialize()
        {
            if (list != null)
                list.CollectionChanged += HandleListCollectionChanged;
        }
        #endregion

        // --------------------------------------------------
        // Properties
        // --------------------------------------------------
        #region Properties
        public ObservableCollection<TModel> List
        {
            get { return list; }
            set { list = value; }
        }

        public UITableViewRowAnimation RowAnimation { get; set; }
        #endregion

        // --------------------------------------------------
        // UITableViewSource Implementation
        // --------------------------------------------------
        #region UITableViewSource Implementation
        public override int NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override int RowsInSection(UITableView tableview, int section)
        {
            if (list == null)
                return 0;
            else
                return list.Count;
        }

        public override float GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            if (list != null)
                return ((IUITableViewCellInfo)list[indexPath.Row]).Height;
            else
                return 44;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            string identifier = ((IUITableViewCellInfo)list[indexPath.Row]).Identifier;

            UITableViewCell cell = tableView.DequeueReusableCell(identifier);
            if (cell == null)
            {
                object obj = Activator.CreateInstance<TCell>();
                IUITableViewCellContext context = obj as IUITableViewCellContext;
                context.DataContext = list[indexPath.Row];

                cell = obj as UITableViewCell;
            }
            return cell;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (RowSelect != null)
                RowSelect(this, list[indexPath.Row]);
        }

        public override void RowDeselected(UITableView tableView, NSIndexPath indexPath)
        {
            if (RowDeselect != null)
                RowDeselect(this, list[indexPath.Row]);
        }
        #endregion

        void HandleListCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            InvokeOnMainThread(delegate {
                if (RowAnimation == UITableViewRowAnimation.None)
                {
                    tableView.ReloadData();
                }
                else
                {
                    tableView.BeginUpdates();
    
                    // Handle change events.
                    switch (e.Action)
                    {
                        case NotifyCollectionChangedAction.Add:
                            List<NSIndexPath> paths = new List<NSIndexPath>();
    
                            int oldCount = list.Count - e.NewItems.Count;
    
                            for(int i=0; i< list.Count - oldCount; i++)
                            {
                                NSIndexPath path = NSIndexPath.FromRowSection(i, 0);
                                paths.Add(path);
                            }
    
                            tableView.InsertRows(paths.ToArray(), RowAnimation);
                            break;
                        case NotifyCollectionChangedAction.Remove:
                            break;
                        case NotifyCollectionChangedAction.Replace:
                            break;
                        case NotifyCollectionChangedAction.Reset:
                            break;
                    }
    
                    tableView.EndUpdates();
                }
            });
        }

    }
}


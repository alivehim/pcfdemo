using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace UtilityTools.Controls
{
    public class ListBoxScroll : ListBox
    {
        public static readonly DependencyProperty IsAutoScrollProperty =
       DependencyProperty.RegisterAttached(
           "IsAutoScroll",
           typeof(bool),
           typeof(ListBoxScroll), new PropertyMetadata(false));

        public bool IsAutoScroll
        {
            get { return (bool)GetValue(IsAutoScrollProperty); }
            set { SetValue(IsAutoScrollProperty, value); }
        }

        public ListBoxScroll() : base()
        {
            SelectionChanged += new SelectionChangedEventHandler(ListBoxScroll_SelectionChanged);
        }

        void ListBoxScroll_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsAutoScroll)
            {
                ScrollIntoView(SelectedItem);
            }
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            if (IsAutoScroll)
            {
                ScrollIntoView(SelectedItem);
            }
            base.OnItemsChanged(e);
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UtilityTools.Controls.Extensions;

namespace UtilityTools.Controls
{
    public class ListBoxHelper
    {
        public static int GetHorizontalScrollDivider(DependencyObject obj)
        {
            return (int)obj.GetValue(HorizontalScrollDividerProperty);
        }
        public static void SetHorizontalScrollDivider(DependencyObject obj, int value)
        {
            obj.SetValue(HorizontalScrollDividerProperty, value);
        }
        public static readonly DependencyProperty HorizontalScrollDividerProperty =
            DependencyProperty.RegisterAttached(
                "HorizontalScrollDivider",
                typeof(int),
                typeof(ListBoxHelper),
                new PropertyMetadata(
                    defaultValue: 0,
                    propertyChangedCallback: (d, e) =>
                    {
                        if (!(d is  ListBox listBox))
                            return;

                        switch ((int)e.OldValue, (int)e.NewValue)
                        {
                            case (0, _):
                                listBox.PreviewMouseWheel += OnPreviewMouseWheel;
                                break;
                            case (_, 0):
                                listBox.PreviewMouseWheel -= OnPreviewMouseWheel;
                                break;
                        }
                    }));

        private static void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var listBox = (ListBox)sender;
            if (!listBox.TryFindDescendant(out ScrollViewer viewer))
                return;

            int divider = GetHorizontalScrollDivider(listBox);

            double offset = viewer.HorizontalOffset;
            offset += e.Delta / (double)divider;

            viewer.ScrollToHorizontalOffset(offset);
        }
    }
}

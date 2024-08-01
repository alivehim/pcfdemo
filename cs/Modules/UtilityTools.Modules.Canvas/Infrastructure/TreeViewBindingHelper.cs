using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using UtilityTools.Controls.JsonViewer;
using UtilityTools.Modules.Canvas.Extensions;

namespace UtilityTools.Modules.Canvas.Infrastructure
{
    public class TreeViewBindingHelper : DependencyObject
    {
        public static string GetText(DependencyObject obj)
        {
            return (string)obj.GetValue(TextProperty);
        }

        public static void SetText(DependencyObject obj, string value)
        {
            obj.SetValue(TextProperty, value);
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.RegisterAttached("Text", typeof(string), typeof(TreeViewBindingHelper), new PropertyMetadata(String.Empty, OnTextChanged));

        private static void OnTextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                var control = sender as JsonViewer;
                if (control != null)
                {
                    control.Load(e.NewValue.ToString());
                    //control.ExpandAll();
                }
            }

        }
    }
}

﻿using System.Windows;
using System.Windows.Controls;

namespace UtilityTools.Controls.Ribbon.UI {
    /// <summary>
    /// Interaction logic for QuickAccessBarItem.xaml
    /// </summary>
    public class QuickAccessBarItem : ContentControl {
        static QuickAccessBarItem() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(QuickAccessBarItem),
             new FrameworkPropertyMetadata(typeof(QuickAccessBarItem)));
        }
    }
}

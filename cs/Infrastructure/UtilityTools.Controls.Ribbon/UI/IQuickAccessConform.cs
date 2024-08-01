﻿using System.Windows.Input;
using System.Windows.Media;

namespace UtilityTools.Controls.Ribbon.UI {
    public interface IQuickAccessConform {
        object ToolTip { get; }
        ImageSource QuickAccessImageSource { get; }
        ICommand Command { get; }
        object CommandParameter { get; }
        string Key { get; }
    }
}

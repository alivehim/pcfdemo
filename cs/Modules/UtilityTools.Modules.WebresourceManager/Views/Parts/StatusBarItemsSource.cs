﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Modules.WebresourceManager.Views.Parts {
    public sealed class StatusBarItemsSource : ObservableCollection<string> {
        public StatusBarItemsSource() {
            Add("Ready");
        }
    }
}

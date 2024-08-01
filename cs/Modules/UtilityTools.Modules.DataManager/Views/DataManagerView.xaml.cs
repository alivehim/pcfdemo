using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UtilityTools.Modules.DataManager.ViewModels;

namespace UtilityTools.Modules.DataManager.Views
{
    /// <summary>
    /// DataManagerView.xaml 的交互逻辑
    /// </summary>
    public partial class DataManagerView : UserControl
    {
        public DataManagerView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var db = this.DataContext as DataManagerViewModel;
            Observable.FromEventPattern<TextChangedEventArgs>(this.fileFilter, "TextChanged").Throttle(TimeSpan.FromSeconds(1)).Subscribe(x =>
                this.Dispatcher.Invoke(() =>
                {
                    var filterText = this.fileFilter.Text;
                    db.NameFilter = filterText;
                }));

        }
    }
}
